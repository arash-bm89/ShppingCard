using Microsoft.AspNetCore.Mvc;

namespace ShoppingCard.Api.Controllers;


[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class BaseController : ControllerBase
{
}