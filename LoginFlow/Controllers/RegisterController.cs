using AutoMapper;
using LoginFlow.Data;
using LoginFlow.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        public RegisterController(ApplicationDbContext context, IMapper mapper)
        {
            _context  = context;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDTO request)
        {
            var user = _mapper.Map<User>(request);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<RegisterDTO>(user);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allUsers = await _context.Users.ToListAsync();
            return Ok(allUsers);
        }
    }

    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
