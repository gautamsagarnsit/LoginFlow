using Microsoft.AspNetCore.Mvc;

namespace LoginFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("An error occurred while processing your request.");
        }
    }
}
