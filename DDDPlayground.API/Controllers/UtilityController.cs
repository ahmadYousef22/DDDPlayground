using Microsoft.AspNetCore.Mvc;

namespace DDDPlayground.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
