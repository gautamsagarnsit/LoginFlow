using AutoMapper;
using LoginFlow.Common;
using LoginFlow.Data;
using LoginFlow.Data.Tables;
using LoginFlow.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace LoginFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private IEmailSender _emailSender;

        public RegisterController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDTO request)
        {
            var user = new IdentityUser { UserName = request.Username, Email = request.Email};
            var result = await _signInManager.UserManager.CreateAsync(user, request.Password);
            if(result.Succeeded)
            {
                await SendConfirmationEmail(user);
                return Ok("Registration Successful");
            }
            return BadRequest($"Registration Failed with errors: {result.Errors}");
        }

        private async Task<IActionResult> SendConfirmationEmail(IdentityUser user)
        { 
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("Confirm Email", "ConfirmEmail", new { userId = user.Id, token }, Request.Scheme);    
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");
            return RedirectToAction("RegisterConfirmation");
        }
    }


    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
