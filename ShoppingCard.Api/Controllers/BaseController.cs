using Microsoft.AspNetCore.Mvc;

namespace ShoppingCard.Api.Controllers;

// todo: make totalCounts static

[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class BaseController : ControllerBase
{
}