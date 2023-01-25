using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ShoppingCard.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;


        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// invoking exceptionHandler middleware to handle exceptions occurred in the lower layers
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="Exception"></exception>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                // logging exception in the logger
                // todo: add serilog to the project
                _logger.LogCritical(e, e.Message);

                // generating internalServerError in response.body
                if (context.Response.HasStarted)
                {
                    throw new Exception("operation already started.");
                }

                // initializing response properties to send to the client
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await SetInternalServerErrorMessage(context.Response);
            }

        }

        /// <summary>
        /// initializing response body error message to the body of the response
        /// </summary>
        /// <param name="response"></param>
        public async Task SetInternalServerErrorMessage(HttpResponse response)
        {
            var internalServerErrorMessageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("InternalServerError has been occurred"));

            await response.Body.WriteAsync(internalServerErrorMessageBytes);

        }
    }
}
