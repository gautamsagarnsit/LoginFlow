using LoginFlow.Data;
using LoginFlow.Data.Tables;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoginFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] UserLoginDTO request)
        {
            var user = await _userManager.FindByNameAsync(request.Username) ?? await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized("User not found");

            var check = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!check)
                return Unauthorized("Incorrect Password");
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);
            if (result.Succeeded)
                return Ok("Login Successful");

            if (result.IsLockedOut)
                return Unauthorized("User is locked out");

            if (result.IsNotAllowed)
                return Unauthorized("User not allowed to sign in (email not confirmed?)");

            return Unauthorized("Invalid Credentials");
        }       
    }

    public class UserLoginDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
