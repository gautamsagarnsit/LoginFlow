using AutoMapper;
using LoginFlow.Common;
using LoginFlow.Data;
using LoginFlow.Data.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace LoginFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        public RegisterController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDTO request)
        {
            var user = new IdentityUser { UserName = request.Username, Email = request.Email};
            var result = await _signInManager.UserManager.CreateAsync(user, request.Password);
            if(result.Succeeded)
            {
                return Ok("Registration Successful");
            }
            return BadRequest($"Registration Failed with errors: {result.Errors}");
        }
    }

    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
