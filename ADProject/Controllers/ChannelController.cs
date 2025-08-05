using ADProject.Models;
using ADProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ADProject.Controllers
{
    [ApiController]
    [Route("api/channel")]
    public class ChannelController : ControllerBase
    {
        private readonly ChannelRepository _repository;

        public ChannelController(ChannelRepository repository)
        {
            _repository = repository;
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
                _repository.CreateChannel(username, dto);
                return Ok("频道创建请求已提交，等待管理员审批");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("review/{id}")]
        public IActionResult ReviewRequest(int id, string status)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            if (userType != "admin")
                return BadRequest("只有admin可以审批活动申请");
            try
            {
                _repository.ReviewChannelRequest(id, status);
                return Ok("频道请求已处理");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("channels/messages")]
        public IActionResult PostChannelMessage([FromBody] PostChannelMessageDto dto)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            if (userType != "organizer")
                return BadRequest("只有organizer可以发布信息");
            var channelId = dto.ChannelId;
            try
            {
                _repository.CreateChannelMessage(channelId, dto, username);
                return Ok("消息已成功发布！");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("channels/join")]
        public IActionResult JoinChannel(int channelId)
        {
            var username = HttpContext.User.Identity?.Name ?? HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("用户未登录");

            try
            {
                _repository.JoinChannel(username, channelId);
                return Ok("已成功加入频道");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("channels/report")]
        public IActionResult ReportChannel([FromBody] ChannelReportDto dto)
        {
            var username = HttpContext.User.Identity?.Name ?? HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("用户未登录");

            try
            {
                _repository.SubmitChannelReport(username, dto.ChannelId, dto.ReportContent);
                return Ok("举报已提交，感谢你的反馈");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("channels/reports/review")]
        public IActionResult ReviewReport(int id, string status)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            if (userType != "admin")
                return BadRequest("只有admin可以审核");
            try
            {
                _repository.ReviewChannelReport(id, status);
                return Ok($"举报已更新为状态：{status}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("channels/getAll")]
        public IActionResult GetAllChannels()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            try
            {
                var channels = _repository.GetAllChannels();
                return Ok(channels);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        [HttpGet("/channel/getChannelById")]
        public IActionResult GetChannelById(int channelId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            try
            {
                var channel = _repository.GetChannelById(channelId);
                if (channel == null)
                    return NotFound("频道不存在");
                return Ok(channel);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        [HttpGet("channels/getChannelMessages")]
        public IActionResult GetChannelMessages(int channelId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            try
            {
                var messages = _repository.GetChannelMessages(channelId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("channels/getChannelMembers")]
        public IActionResult GetChannelMembers(int channelId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            try
            {
                var members = _repository.GetChannelMembers(channelId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("channels/getUserJoinedChannel")]
        public IActionResult GetUserJoinedChannels()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            try
            {
                var channels = _repository.GetUserJoinedChannels(username);
                return Ok(channels);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("/getOrganizerOwnedChannel")]
        public IActionResult GetOrganizerOwnedChannels()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            try
            {
                var channels = _repository.GetOrganizerOwnedChannel(username);
                return Ok(channels);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("/updateChannel")]
        public IActionResult UpdateChannel([FromBody] UpdateChannelDto dto)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            try
            {
                _repository.UpdateChannel(dto, username);
                return Ok("频道信息已更新");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

