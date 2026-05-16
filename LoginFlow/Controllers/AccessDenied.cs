using Microsoft.AspNetCore.Mvc;

namespace LoginFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessDenied : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Your are not allowed to access the content, signIn to Access");
        }
    }
}
