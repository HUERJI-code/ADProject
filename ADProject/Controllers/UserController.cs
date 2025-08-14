using ADProject.Models;
using ADProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ADProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _repository;

        public UserController(UserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            var users = _repository.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = _repository.GetById(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        [Route("CreateUser")]
        public IActionResult CreateUser(User user)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!emailRegex.IsMatch(user.Email))
            {
                return BadRequest("Invalid email format");
            }

            if (_repository.ExistsByEmail(user.Email))
                return BadRequest("Email already exists");
            if (_repository.ExistsByName(user.Name))
                return BadRequest("Username already exists");
            User newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = "user"
            };
            _repository.Add(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.UserId }, newUser);
        }

        [HttpPost]
        [Route("CreateOrganizer")]
        public IActionResult CreateOrganizer(User user)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!emailRegex.IsMatch(user.Email))
            {
                return BadRequest("Invalid email format");
            }

            if (_repository.ExistsByEmail(user.Email))
                return BadRequest("Email already exists");
            if (_repository.ExistsByName(user.Name))
                return BadRequest("Username already exists");
            User newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = "organizer"
            };
            _repository.Add(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.UserId }, newUser);
        }

        [HttpPut("/updateUserPass")]
        public IActionResult Update([FromBody] UserUpdateDto userUpdateDto)
        {
            var message = _repository.Update(userUpdateDto);
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }

        [HttpPut("/banUser")]
        public IActionResult BanUser(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username is null)
            {
                return Unauthorized("Please log in first");
            }
            if (HttpContext.Session.GetString("UserType") != "admin")
            {
                return Unauthorized("Only admin can ban users");
            }
            _repository.banUser(id);
            return Ok("User has been banned");
        }

        [HttpPut("/UnbanUser")]
        public IActionResult UnBanUser(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username is null)
            {
                return Unauthorized("Please log in first");
            }
            if (HttpContext.Session.GetString("UserType") != "admin")
            {
                return Unauthorized("Only admin can unban users");
            }
            _repository.UnbanUser(id);
            return Ok("User has been unbanned");
        }
    }
}
