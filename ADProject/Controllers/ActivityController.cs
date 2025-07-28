using ADProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using ADProject.Models;

namespace ADProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly ActivityRepository _repository;

        public ActivityController(ActivityRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("create")]
        public IActionResult CreateActivityWithRegistration([FromBody] CreateActivityDto dto)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户，无法创建活动");
            if (userType != "organizer")
                return BadRequest("只有组织者可以创建活动");
            try
            {
                _repository.CreateActivityWithRegistration(username, dto);
                return Ok(new { message = "活动已创建并自动注册成功", dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var activity = _repository.GetById(id);
            return activity == null ? NotFound() : Ok(activity);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpPut]
        [Route("update/{activityId}")]
        public IActionResult UpdateActivity(int activityId, [FromBody] UpdateActivityDto dto)
        {
            try
            {
                _repository.UpdateActivity(activityId, dto);
                return Ok(new { message = "活动更新成功。" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("approve/{requestId}")]
        public IActionResult ApproveRequest(int requestId, [FromQuery] string status)
        {
            try
            {
                _repository.ApproveActivityRequest(requestId, status);
                return Ok(new { message = $"申请已更新为 {status}" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
