using Microsoft.AspNetCore.Mvc;

namespace ShoppingCard.Api.Controllers
{
    public class BaseController: ControllerBase
    {
        public CancellationToken RequestAbborted { get; set; }
    }
}
