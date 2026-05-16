using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using LoginFlow.Controllers;
using Xunit;

namespace LoginFlow.Tests
{
    public class RegisterControllerTests
    {
        [Fact]
        public async Task Should_Create_User()
        {
            var userStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var signInManager = new Mock<SignInManager<IdentityUser>>(userManager.Object, null, null, null, null, null, null);
            var emailSender = new Mock<IEmailSender>();

            var controller = new RegisterController(userManager.Object, signInManager.Object, emailSender.Object);

            var dto = new RegisterDTO { Username = "testuser", Email = "test@example.com", Password = "P@ssw0rd!" };

            // Setup CreateAsync to return Success
            signInManager.Setup(s => s.UserManager.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                         .ReturnsAsync(IdentityResult.Success);

            var result = await controller.Register(dto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Should_Reject_Duplicate_Email()
        {
            var userStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var signInManager = new Mock<SignInManager<IdentityUser>>(userManager.Object, null, null, null, null, null, null);
            var emailSender = new Mock<IEmailSender>();

            var controller = new RegisterController(userManager.Object, signInManager.Object, emailSender.Object);

            var dto = new RegisterDTO { Username = "testuser", Email = "existing@example.com", Password = "P@ssw0rd!" };

            var errors = new[] { new IdentityError { Code = "DuplicateEmail", Description = "Email already exists" } };
            var failedResult = IdentityResult.Failed(errors);

            signInManager.Setup(s => s.UserManager.CreateAsync(It.IsAny<IdentityUser>(), dto.Password))
                         .ReturnsAsync(failedResult);

            var result = await controller.Register(dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
