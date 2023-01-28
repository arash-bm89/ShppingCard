using System.Net;
using Newtonsoft.Json;
using ShoppingCard.Service.IServices;
using System.Text;

namespace ShoppingCard.Api.Middlewares
{
    public class CustomAuthorizationMiddleware
    {
        private readonly IJwtService _jwtService;
        private readonly RequestDelegate _next;
        private readonly string _jwtKey;

        public CustomAuthorizationMiddleware(IJwtService jwtService, RequestDelegate next, IConfiguration configuration)
        {
            _jwtService = jwtService;
            _next = next;
            _jwtKey = configuration.GetSection("keys").GetValue<string>("jwtKey");
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value
                .Split("/");

            if (path[1] == "user")
            {
                await _next.Invoke(context);
                return;
            }

            bool noAuthorizationHeader = !context.Request.Headers.TryGetValue("Authorization", out var tokenWithSchema);
            if (noAuthorizationHeader)
            {
                await SetUnauthorizedMessage(context.Response);
                return;
            }

            var parts = tokenWithSchema.ToString().Split(' ');

            if (parts.Length != 2 || parts[0].ToLower() != "bearer")
            {
                await SetUnauthorizedMessage(context.Response);
                return;
            }

            var token = parts[1];

            var isTokenValid = _jwtService.ValidateJwt(token, _jwtKey);

            if (!isTokenValid)
            {
                await SetUnauthorizedMessage(context.Response);
                return;
            }

            // todo: add jwtObject to the header
            var tokenPayload = token.Split('.')[1];
            context.Request.Headers.Add("token-payload", tokenPayload);

            await _next.Invoke(context);

        }

        private async Task SetUnauthorizedMessage(HttpResponse response)
        {
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            var unAuthorizedErrorMessageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("UnAuthorized, Please Login First"));

            await response.Body.WriteAsync(unAuthorizedErrorMessageBytes);
        }
    }
}
