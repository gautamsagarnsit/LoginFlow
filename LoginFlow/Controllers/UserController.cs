using Microsoft.AspNetCore.Mvc;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // POST api/user
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            // Normally you'd save to a database here
            return Ok($"User {user.Name} with email {user.Email} created successfully!");
        }

        // POST api/user/form
        [HttpPost("form")]
        public IActionResult CreateUser([FromForm] UserFormDto user)
        {
            if (user == null)
            {
                return BadRequest("Form data is required.");
            }

            // Normally you'd save to a database here
            return Ok($"User {user.Name} with email {user.Email} created successfully!");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadProfile([FromForm] UserUploadDto user, [FromServices] IWebHostEnvironment env)
        {
            if (user.ProfilePicture == null || user.ProfilePicture.Length == 0)
            {
                return BadRequest("Profile picture is required.");
            }

            // Create a safe uploads folder inside wwwroot
            var uploadsFolder = Path.Combine("uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }


            // Save file safely
            var fileName = Path.GetFileName(user.ProfilePicture.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await user.ProfilePicture.CopyToAsync(stream);
            }

            // Build a URL to access the file
            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            return Ok(new
            {
                Name = user.Name,
                ProfilePictureUrl = fileUrl
            });

        }

        // Example action using FromServices
        [HttpGet("time")]
        public IActionResult GetServerTime([FromServices] ITimeService timeService)
        {
            var currentTime = timeService.GetCurrentTime();
            return Ok(new { ServerTime = currentTime });
        }

        [HttpGet("search")]
        public IActionResult Search([AsParameters] SearchParams parameters)
        {
            return Ok($"Searching for {parameters.Keyword}, page {parameters.Page}, size {parameters.PageSize}");
        }



    }

    // DTO class for request body
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    // DTO class for form data
    public class UserFormDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    // DTO for form data + file
    public class UserUploadDto
    {
        public string Name { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }

    // Example service interface
    public interface ITimeService
    {
        string GetCurrentTime();
    }

    // Implementation
    public class TimeService : ITimeService
    {
        public string GetCurrentTime() => DateTime.Now.ToString("F");
    }
    public class SearchParams
    {
        public string Keyword { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }



}