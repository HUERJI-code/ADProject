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
                return Unauthorized("Unauthenticated user, unable to create channel");
            if (userType != "organizer")
                return BadRequest("Only organizers can create channels");
            try
            {
                _repository.CreateChannel(username, dto);
                return Ok("Channel creation request has been submitted and is awaiting admin approval");
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
                return Unauthorized("Unauthenticated user");
            if (userType != "admin")
                return BadRequest("Only admin can approve channel requests");
            try
            {
                _repository.ReviewChannelRequest(id, status);
                return Ok("Channel request processed successfully");
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
                return Unauthorized("Unauthenticated user");
            if (userType != "organizer")
                return BadRequest("Only organizers can post messages");
            var channelId = dto.ChannelId;
            try
            {
                _repository.CreateChannelMessage(channelId, dto, username);
                return Ok("Message posted successfully");
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
                return Unauthorized("Unauthenticated user");
            try
            {
                _repository.JoinChannel(username, channelId);
                return Ok("Joined channel successfully");
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
                return Unauthorized("Unauthenticated user");
            try
            {
                _repository.SubmitChannelReport(username, dto.ChannelId, dto.ReportContent);
                return Ok("Report submitted, thank you for your feedback");
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
                return Unauthorized("Unauthenticated user");
            if (userType != "admin")
                return BadRequest("Only admin can review reports");
            try
            {
                _repository.ReviewChannelReport(id, status);
                return Ok($"Report status updated to: {status}");
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
                return Unauthorized("Unauthenticated user");
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
                return Unauthorized("Unauthenticated user");
            try
            {
                var channel = _repository.GetChannelById(channelId);
                if (channel == null)
                    return NotFound("Channel not found");
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
                return Unauthorized("Unauthenticated user");
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
                return Unauthorized("Unauthenticated user");
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
                return Unauthorized("Unauthenticated user");
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
                return Unauthorized("Unauthenticated user");
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
                return Unauthorized("Unauthenticated user");
            try
            {
                _repository.UpdateChannel(dto, username);
                return Ok("Channel information updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("/banChannel")]
        public IActionResult BanChannel(int channelId)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Unauthenticated user");
            if (userType != "admin")
                return BadRequest("Only admin can ban channels");
            try
            {
                _repository.banChannel(channelId);
                return Ok("Channel has been banned");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("/unbanChannel")]
        public IActionResult UnbanChannel(int channelId)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Unauthenticated user");
            if (userType != "admin")
                return BadRequest("Only admin can unban channels");
            try
            {
                _repository.UnbanChannel(channelId);
                return Ok("Channel has been unbanned");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("/getAllChannelReports")]
        public IActionResult GetAllChannelReports()
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Unauthenticated user");
            if (userType != "admin")
                return BadRequest("Only admin can view channel reports");
            try
            {
                var reports = _repository.GetAllChannelReports();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("/getChannelRequests")]
        public IActionResult GetChannelRequests()
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Unauthenticated user");
            if (userType != "admin")
                return BadRequest("Only admin can view channel requests");
            try
            {
                var requests = _repository.GetAllChannelRequests();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("quitChannel")]
        public IActionResult QuitChannel(int channelId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Unauthenticated user");
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "user")
                return BadRequest("Only normal users can quit channels");
            try
            {
                _repository.quitChannel(username, channelId);
                return Ok("Quit channel successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("/cancelChannel")]
        public IActionResult CancelChannel(int channelId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Unauthenticated user");
            var userType = HttpContext.Session.GetString("UserType");
            if (userType != "organizer")
                return BadRequest("Only organizer can cancel channel");
            try
            {
                _repository.cancelChannel(channelId);
                return Ok("Channel has been cancelled");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
