using LoginFlow.Data.Tables;
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

            if (request.Username == "admin" && request.PasswordHash == "password")
            {
                return Ok($"Login Successful: {request.Username}, {request.PasswordHash}, {request.Email}");
            }
            return Ok($"Login Failed: {request.Username}, {request.PasswordHash}");
        }
    }
}
