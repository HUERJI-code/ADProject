using ADProject.Models;
using ADProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ADProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly UserRepository _repository;

        public LoginController(UserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var identifier = loginDto.Identifier?.Trim();

            // Find user by email or username
            var user = _repository.GetAll().FirstOrDefault(u =>
                (u.Email == identifier || u.Name == identifier) &&
                u.PasswordHash == loginDto.PasswordHash);

            if (user is null)
            {
                return Unauthorized("Username or email does not exist, or password is incorrect");
            }
            if (user.status != "active")
            {
                return Unauthorized("User is banned");
            }

            // Write to session
            HttpContext.Session.SetString("Username", user.Name);
            HttpContext.Session.SetString("UserType", user.Role);

            return Ok(new
            {
                message = "Login successful",
                username = user.Name
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("check")]
        public IActionResult CheckLogin()
        {
            // Check if username exists in session
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Not logged in");
            }
            return Ok(new { message = "Logged in", username });
        }

        [HttpGet("/checkLoginUserType")]
        public IActionResult CheckLoginUserType()
        {
            // Check if user type exists in session
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(userType))
            {
                return Unauthorized("Not logged in");
            }
            return Ok(new { message = "Logged in", userType });
        }

        [HttpGet("/health")]
        public IActionResult HealthCheck()
        {
            // Health check endpoint
            return Ok(new { message = "API is healthy" });
        }

        [HttpGet("getInviteCode")]
        public IActionResult GetInviteCode()
        {
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(userType) || userType != "admin")
            {
                return Unauthorized("Only admin can get invite code");
            }
            // Get invite code
            var inviteCode = _repository.GenerizeInviteCode();
            if (inviteCode == null)
            {
                return NotFound("Invite code does not exist");
            }
            return Ok(new { code = inviteCode });
        }

        [HttpGet("useInviteCode")]
        public IActionResult UseInviteCode(string code)
        {
            // Use invite code
            try
            {
                _repository.UseInviteCode(code);
                return Ok(new { message = "Invite code used successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("/test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Test endpoint is working" });
        }
    }
}
