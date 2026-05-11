using LoginFlow.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private ApplicationDbContext _context;
        public RegisterController(ApplicationDbContext context)
        {
            _context  = context;
        }
        [HttpPost]
        public IActionResult Register([FromForm] User request)
        {
            _context.Users.Add(request);
            _context.SaveChanges();
            return Ok(request);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Users.ToList());
        }
    }
}
