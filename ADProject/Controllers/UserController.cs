using ADProject.Models;
using ADProject.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
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
                return Unauthorized("请先登录！");
            }
            if (HttpContext.Session.GetString("UserType") != "admin")
            {
                return Unauthorized("只有管理员可以封禁用户！");
            }
            _repository.banUser(id);
            return Ok("用户已被封禁");
        }

        [HttpPut("/UnbanUser")]
        public IActionResult UnBanUser(int id)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username is null)
            {
                return Unauthorized("请先登录！");
            }
            if (HttpContext.Session.GetString("UserType") != "admin")
            {
                return Unauthorized("只有管理员可以解封用户！");
            }
            _repository.UnbanUser(id);
            return Ok("用户已被解封");
        }
    }
}
