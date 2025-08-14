using ADProject.Models;
using ADProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ADProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileRepository _repository;

        public UserProfileController(UserProfileRepository repository)
        {
            _repository = repository;
        }

        // PUT: Update or create Profile
        [HttpPut("update")]
        public IActionResult UpdateProfile([FromBody] UpdateUserProfileDto dto)
        {
            // Get the current username from session
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Not logged in, unable to update profile");

            try
            {
                _repository.UpsertProfile(username, dto);
                return Ok(new { message = "Profile has been updated or created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: Get the current user's Profile
        [HttpGet("me")]
        public IActionResult GetMyProfile()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Not logged in, unable to retrieve profile");

            var profile = _repository.GetProfileByUsername(username);
            if (profile == null)
                return NotFound("User profile has not been created yet");

            return Ok(profile);
        }
    }
}
