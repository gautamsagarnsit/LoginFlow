using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private UserManager<IdentityUser> _userManager;
        private IEmailSender _emailSender;
        public PasswordResetController(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }


        [HttpPost]
        public async Task<IActionResult> RequestPasswordReset([FromForm] PasswordResetDTO request)
        {
            var user = await _userManager.FindByNameAsync(request.Username) ??await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return NotFound("User not found");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "PasswordReset", new { userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(request.Email, "Password Reset Request", $"Click the link to reset your password: {resetLink}");
            return Ok("Password reset link has been sent to your email.");
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword(string userId, string token, [FromForm] string Password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");
            var passwordReset = await _userManager.ResetPasswordAsync(user, token, Password);
            if(passwordReset.Succeeded)
                return Ok("Password reset successful");
            return Ok("Password reset failed");
        }

    }

    public class PasswordResetDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
