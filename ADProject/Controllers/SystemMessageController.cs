using Microsoft.AspNetCore.Mvc;
using ADProject.Repositories;
using ADProject.Models;

namespace ADProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemMessageController : Controller
    {
        private readonly SystemMessageRepository _repository;

        public SystemMessageController(SystemMessageRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var messages = _repository.GetAll();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var message = _repository.GetById(id);
            if (message == null)
                return NotFound();
            return Ok(message);
        }

        [HttpGet("/getLoginUserMessage")]
        public IActionResult GetByUserName()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Unauthenticated user, unable to retrieve messages");
            var messages = _repository.GetByUserName(username);
            if (messages == null || !messages.Any())
                return NotFound("No related messages found");
            return Ok(messages);
        }

        [HttpPost("create")]
        public IActionResult Create(CreateSystemMessageDto createSystemMessageDto)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Unauthenticated user, unable to create message");
            if (createSystemMessageDto == null)
                return BadRequest("Message content cannot be empty");
            try
            {
                _repository.Create(createSystemMessageDto);
                return Ok("Message created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create message: {ex.Message}");
            }
        }

        [HttpPost("/MarkAsRead")]
        public IActionResult MarkAsRead(List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("Message ID list cannot be empty");
            try
            {
                foreach (var id in ids)
                {
                    _repository.MarkAsRead(id);
                }
                return Ok("Messages marked as read");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to mark messages as read: {ex.Message}");
            }
        }

        [HttpPost("/MarkAsRead/{id}")]
        public IActionResult MarkAsRead(int id)
        {
            try
            {
                _repository.MarkAsRead(id);
                return Ok("Message marked as read");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to mark message as read: {ex.Message}");
            }
        }
    }
}
