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
                return BadRequest("邮箱格式不正确！");
            }

            if (_repository.ExistsByEmail(user.Email))
                return BadRequest("邮箱已存在！");
            if (_repository.ExistsByName(user.Name))
                return BadRequest("用户名已存在！");
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
                return BadRequest("邮箱格式不正确！");
            }

            if (_repository.ExistsByEmail(user.Email))
                return BadRequest("邮箱已存在！");
            if (_repository.ExistsByName(user.Name))
                return BadRequest("用户名已存在！");
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

        [HttpPut("{id}")]
        public IActionResult Update(int id, User user)
        {
            if (id != user.UserId) return BadRequest();
            user.Role = "user"; // Ensure role is set to 'user' on update
            _repository.Update(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }
    }
}
