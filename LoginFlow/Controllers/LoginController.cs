using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LoginFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login([FromForm] User request)
        {

            if (request.Username == "admin" && request.Password == "password")
            {
                return Ok($"Login Successful: {request.Username}, {request.Password}, {request.Email}");
            }
            return Ok($"Login Failed: {request.Username}, {request.Password}");
        }
    }
}
