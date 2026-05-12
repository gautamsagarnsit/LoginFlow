using AutoMapper;
using LoginFlow.Data;
using LoginFlow.Data.Tables;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LoginFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        public LoginController(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] UserLoginDTO request)
        {
            User loginRequest = _mapper.Map<User>(request);
            User? user = _context.Users.Where(u => u.Username == loginRequest.Username && u.Email == loginRequest.Email && u.PasswordHash == loginRequest.PasswordHash ).FirstOrDefault();
            if(user != null)
            {
                return Ok($"Login Successful: {request.Username}, {request.Email}");
            }
            return Ok($"Login Failed: {request.Username}, {request.Password}");
        }       
    }

    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
