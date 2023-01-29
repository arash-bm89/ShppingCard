using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingCard.Service.IServices;
using ShoppingCard.Service.Services;

namespace ShoppingCard.Api.Filters
{
    public class AdminPrivilageActionFilter : ActionFilterAttribute
    {
        private readonly string _adminSecret;

        public AdminPrivilageActionFilter(string adminSecret)
        {
            _adminSecret = adminSecret;
        }
        // todo what is ActionExecutingContext
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var noSecret = !context.HttpContext.Request.Headers.TryGetValue("Secret", out var secret);

            if (noSecret)
            {
                SetUnAuthorizedResult(context);
                return;
            }

            if (!string.Equals(secret, _adminSecret))
            {
                SetUnAuthorizedResult(context);
                return;
            }

            base.OnActionExecuting(context);

        }

        private void SetUnAuthorizedResult(ActionExecutingContext context)
        {
            context.Result = new UnauthorizedObjectResult("You Can't Access this method");
        }
    }
}
