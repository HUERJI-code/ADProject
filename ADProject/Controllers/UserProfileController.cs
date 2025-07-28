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

        // PUT: 修改或新建 Profile
        [HttpPut("update")]
        public IActionResult UpdateProfile([FromBody] UpdateUserProfileDto dto)
        {
            // 👇 从 Session 获取当前用户名
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录，无法修改资料");

            try
            {
                _repository.UpsertProfile(username, dto);
                return Ok(new { message = "资料已更新或创建成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: 获取当前用户的 Profile
        [HttpGet("me")]
        public IActionResult GetMyProfile()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录，无法获取资料");

            var profile = _repository.GetProfileByUsername(username);
            if (profile == null)
                return NotFound("尚未创建用户资料");

            return Ok(profile);
        }
    }

}
