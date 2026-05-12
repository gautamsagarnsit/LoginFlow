using AutoMapper;
using LoginFlow.Common;
using LoginFlow.Data;
using LoginFlow.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

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
            var CheckUsername = await _context.Users.Where(b =>b.Username == request.Username).ToListAsync();
            var checkEmail = await _context.Users.Where(b => b.Email == request.Email).ToListAsync();
            if (CheckUsername.Any())
            {
                return Ok($"{request.Username} already exist");
            }
            if(checkEmail.Any())
            {
                return Ok($"{request.Email} already Exist");
            }
            var user = _mapper.Map<User>(request);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<RegisterDTO>(user);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<User> allUsers = await _context.Users.ToListAsync();
            var response = _mapper.Map<List<UserResponseDTO>>(allUsers);
            return Ok(response);
        }
    }

    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
