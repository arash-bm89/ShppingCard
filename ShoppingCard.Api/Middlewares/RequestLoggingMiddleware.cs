using System.Text;
using Athena.RabbitMQHelper;
using ShoppingCard.BrokerMessage;

namespace ShoppingCard.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;


        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IAsyncJobProducer<LogMessage> logProducer)
        {
            // logMessage and originalBody and memStream are going to use in try block and finally block. so we initialize them in here.

            // initialize logMessage
            // can use in try, catch and finally
            var logMessage = new LogMessage()
            {
                Ip = context.Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString(),
                HttpMethod = context.Request.Method,
            };

            // start copying originalBody and working with a new memoryStream
            // because we can not use body.Seek() or body.position = x, it is not enabled on the originalResponse stream
            var originalBody = context.Response.Body;

            await using var memStream = new MemoryStream();

            context.Response.Body = memStream;

            try
            {
                // enable buffering on request so we can reading stream of it
                context.Request.EnableBuffering();

                // reset the stream position
                context.Request.Body.Position = 0;

                // read all of request body
                logMessage.Body = await new StreamReader(context.Request.Body, Encoding.UTF8).ReadToEndAsync();

                // reset the stream to the default position again so other services can work with it.
                context.Request.Body.Position = 0;


                // if an error going to be throw, it will be at this point
                await _next.Invoke(context);

                logMessage.StatusCode = context.Response.StatusCode;

            }
            catch (Exception e)
            {
                logMessage.StatusCode = 500;
                logMessage.HasException = true;
                logMessage.ErrorMessage = e.Message;
                throw;
            }
            finally
            {
                // reset memStream (that going to be replace by originalBody) to the 0 offset that client can read it.
                memStream.Position = 0;

                // copy the temporary memoryStream to originalBody that we can use originalResponse body default property values.
                await memStream.CopyToAsync(originalBody);

                // initializing body with originalBody's properties
                context.Response.Body = originalBody;

                // if an exception throw happened, then we don't have body then.
                await logProducer.PublishAsync(logMessage);

            }
        }

        public async Task<string> GetResponseBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
}
