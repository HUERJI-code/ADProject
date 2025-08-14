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
                return Unauthorized("未登录用户，无法获取消息");
            var messages = _repository.GetByUserName(username);
            if (messages == null || !messages.Any())
                return NotFound("没有找到相关消息");
            return Ok(messages);
        }

        [HttpPost("create")]
        public IActionResult Create(CreateSystemMessageDto createSystemMessageDto)
        {
            
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户，无法创建消息");
            if (createSystemMessageDto == null)
                return BadRequest("消息内容不能为空");
            try
            {
                _repository.Create(createSystemMessageDto);
                return Ok("消息创建成功");
            }
            catch (Exception ex)
            {
                return BadRequest($"创建消息失败: {ex.Message}");
            }
        }

        [HttpPost("/MarkAsRead")]
        public IActionResult MarkAsRead(List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("消息ID列表不能为空");
            try
            {
                foreach (var id in ids)
                {
                    _repository.MarkAsRead(id);
                }
                return Ok("消息已标记为已读");
            }
            catch (Exception ex)
            {
                return BadRequest($"标记消息为已读失败: {ex.Message}");
            }
        }
        [HttpPost("/MarkAsRead/{id}")]
        public IActionResult MarkAsRead(int id)
        {
            try
            {
                _repository.MarkAsRead(id);
                return Ok("消息已标记为已读");
            }
            catch (Exception ex)
            {
                return BadRequest($"标记消息为已读失败: {ex.Message}");
            }

        }
    }
}
