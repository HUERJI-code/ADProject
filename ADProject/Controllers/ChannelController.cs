using ADProject.Models;
using ADProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ADProject.Controllers
{
    [ApiController]
    [Route("api/channel")]
    public class ChannelController : ControllerBase
    {
        private readonly ChannelRepository _service;

        public ChannelController(ChannelRepository service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public IActionResult CreateChannel([FromBody] CreateChannelDto dto)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户，无法创建活动");
            if (userType != "organizer")
                return BadRequest("只有组织者可以创建活动");
            try
            {
                _service.CreateChannel(username, dto);
                return Ok("频道创建请求已提交，等待管理员审批");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPut("review/{id}")]
        //public IActionResult ReviewRequest(int id, [FromBody] ReviewRequestDto dto)
        //{
        //    try
        //    {
        //        _service.ReviewChannelRequest(id, dto);
        //        return Ok("频道请求已处理");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
