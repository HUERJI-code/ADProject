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

            // 查找用户：邮箱或用户名
            var user = _repository.GetAll().FirstOrDefault(u =>
                (u.Email == identifier || u.Name == identifier) &&
                u.PasswordHash == loginDto.PasswordHash);

            if (user is null)
            {
                return Unauthorized("用户名或邮箱不存在，或密码错误！");
            }

            // 写入会话
            HttpContext.Session.SetString("Username", user.Name);
            HttpContext.Session.SetString("UserType", user.Role);

            return Ok(new
            {
                message = "登录成功",
                username = user.Name
            });
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // 清除 Session
            HttpContext.Session.Clear();
            return Ok(new { message = "已登出！" });
        }

        [HttpGet("check")]
        public IActionResult CheckLogin()
        {
            // 检查 Session 中是否有用户名
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("未登录！");
            }
            return Ok(new { message = "已登录", username });
        }

    }
}
