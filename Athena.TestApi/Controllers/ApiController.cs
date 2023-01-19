using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Athena.TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IDatabase _db;
        private readonly string _prefix = "post";

        public ApiController(IDatabase db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> post(string key, string value)
        {
            await _db.StringSetAsync($"{_prefix} - {key}", value);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> get(string key)
        {
            var value = await _db.StringGetAsync($"{_prefix} - {key}");
            return Ok(value.ToString());
        }
    }
}
