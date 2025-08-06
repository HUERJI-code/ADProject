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
                Console.WriteLine("Error creating activity: " + ex.Message);
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
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            if (userType != "admin")
                return BadRequest("只有admin可以审批活动申请");
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

        [HttpPost("register/{activityId}")]
        public IActionResult RegisterForActivity(int activityId)
        {

            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");

            try
            {
                _repository.RegisterForActivity(username, activityId);
                return Ok("申请已提交");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("review/{requestId}")]
        public IActionResult ReviewActivityRegistration(int requestId, [FromBody] string status)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            if (userType != "organizer")
                return BadRequest("只有组织者可以审批用户注册申请");
            try
            {
                _repository.ReviewRegistrationRequest(requestId, status);
                return Ok("审核已完成");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("activities/search")]
        public IActionResult SearchActivities([FromQuery] string keyword)
        {
            try
            {
                var results = _repository.SearchActivitiesByKeyword(keyword);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("activities/favourite/{activityId}")]
        public IActionResult AddToFavourites(int activityId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录");

            try
            {
                _repository.AddToFavourites(username, activityId);
                return Ok("已成功收藏该活动");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("activities/favourite/{activityId}")]
        public IActionResult RemoveFromFavourites(int activityId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录");

            try
            {
                _repository.RemoveFromFavourites(username, activityId);
                return Ok("已取消收藏");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("favorites")]
        public ActionResult<List<Activity>> GetFavorites()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "未登录用户" });

            try
            {
                var activities = _repository.GetFavouriteActivitiesByUsername(username);
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("registered")]
        public ActionResult<List<Activity>> GetRegistered()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "未登录用户" });

            try
            {
                var activities = _repository.GetRegisteredActivitiesByUsername(username);
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("/getLoginOrganizerActivities")]
        public ActionResult<List<Activity>> GetLoginOrganizerActivities()
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "未登录用户" });
            if (userType != "organizer")
                return BadRequest(new { message = "只有组织者可以查看自己的活动" });
            try
            {
                var activities = _repository.GetLoginOrganizerActivities(username);
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }

        }

        [HttpGet("/getOrganizerActivityRegisterRequest")]
        public ActionResult<List<Activity>> GetOrganizerActivityRegisterRequest()
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "未登录用户" });
            if (userType != "organizer")
                return BadRequest(new { message = "只有组织者可以查看注册申请" });
            try
            {
                var activities = _repository.GetOrganizerRegistrationRequests(username);
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("/checkActivityNumber")]
        public ActionResult<int> CheckActivityNumber(int activityId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "未登录用户" });
            try
            {
                int count = _repository.GetActivityCountByActivityId(activityId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("/banActivity")]
        public IActionResult BanActivity(int activityId)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            if (userType != "admin")
                return BadRequest("只有管理员可以禁用活动");
            try
            {
                _repository.banActivity(activityId);
                return Ok(new { message = "活动已被禁用" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("/unbanActivity")]
        public IActionResult UnbanActivity(int activityId)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            if (userType != "admin")
                return BadRequest("只有管理员可以启用活动");
            try
            {
                _repository.UnbanActivity(activityId);
                return Ok(new { message = "活动已被启用" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("/getAllActivityRequest")]
        public IActionResult GetAllActivityRequest()
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("未登录用户！");
            if (userType != "admin")
                return BadRequest("只有管理员可以查看所有活动申请");
            try
            {
                var requests = _repository.GetAllActivityRequests();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
