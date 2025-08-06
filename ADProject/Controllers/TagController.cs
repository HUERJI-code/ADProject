using ADProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using ADProject.Models;

namespace ADProject.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class TagController : ControllerBase
        {
            private readonly TagRepository _tagRepository;

            public TagController(TagRepository tagRepository)
            {
                _tagRepository = tagRepository;
            }

            // GET: api/tag
            [HttpGet]
            public IActionResult GetAll()
            {
                var tags = _tagRepository.GetAllTags();
                return Ok(tags);
            }

            // GET: api/tag/{id}
            [HttpGet("{id}")]
            public IActionResult Get(int id)
            {
                var tag = _tagRepository.GetById(id);
                if (tag == null)
                    return NotFound($"标签 ID {id} 不存在！");
                return Ok(tag);
            }

            // GET: api/tag/profile/{profileId}
            [HttpGet("profile/{profileId}")]
            public IActionResult GetTagsByProfile(int profileId)
            {
                var tags = _tagRepository.GetTagsByUserProfileId(profileId);
                return Ok(tags);
            }

            // GET: api/tag/activity/{activityId}
            [HttpGet("activity/{activityId}")]
            public IActionResult GetTagsByActivity(int activityId)
            {
                var tags = _tagRepository.GetTagsByActivityId(activityId);
                return Ok(tags);
            }

        // POST: api/tag
        [HttpPost("/createTag")]
        public IActionResult Create([FromBody] Tag tag)
        {
            if (tag == null || string.IsNullOrWhiteSpace(tag.Name))
            {
                return BadRequest("标签名称不能为空！");
            }
            if (_tagRepository.TagExists(tag.Name))
            {
                return BadRequest($"标签 '{tag.Name}' 已存在！");
            }
            var newtag = new Tag
            {
                Name = tag.Name,
                Description = tag.Description
            };
            _tagRepository.AddTag(newtag);
            return CreatedAtAction(nameof(Get), new { id = tag.TagId }, tag);
        }
    }

}
