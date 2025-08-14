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
        private readonly UserRepository _userRepository;

        public ActivityController(ActivityRepository repository, UserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        [HttpPost("create")]
        public IActionResult CreateActivityWithRegistration([FromBody] CreateActivityDto dto)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Unauthenticated user, unable to create activity");
            if (userType != "organizer")
                return BadRequest("Only organizers can create activities");
            try
            {
                _repository.CreateActivityWithRegistration(username, dto);
                return Ok(new { message = "Activity has been created and automatically registered successfully", dto });
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
                return Ok(new { message = "Activity updated successfully" });
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
                return Unauthorized("Unauthenticated user");
            if (userType != "admin")
                return BadRequest("Only admin can approve activity requests");
            try
            {
                _repository.ApproveActivityRequest(requestId, status);
                return Ok(new { message = $"Request has been updated to {status}" });
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
                return Unauthorized("Unauthenticated user");
            try
            {
                _repository.RegisterForActivity(username, activityId);
                return Ok("Application submitted");
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
                return Unauthorized("Unauthenticated user");
            if (userType != "organizer")
                return BadRequest("Only organizers can review user registration requests");
            try
            {
                _repository.ReviewRegistrationRequest(requestId, status);
                return Ok("Review completed");
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
                return Unauthorized("Unauthenticated user");
            try
            {
                _repository.AddToFavourites(username, activityId);
                return Ok("Activity added to favourites successfully");
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
                return Unauthorized("Unauthenticated user");
            try
            {
                _repository.RemoveFromFavourites(username, activityId);
                return Ok("Activity removed from favourites");
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
                return Unauthorized(new { message = "Unauthenticated user" });
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
                return Unauthorized(new { message = "Unauthenticated user" });
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

        [HttpGet("/GetRegisteredActivitiesByUserId")]
        public ActionResult<List<Activity>> GetRegisteredActivitiesByUserId(int userId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "Unauthenticated user" });
            try
            {
                var activities = _repository.GetRegisteredActivitiesByUserId(userId);
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
                return Unauthorized(new { message = "Unauthenticated user" });
            if (userType != "organizer")
                return BadRequest(new { message = "Only organizers can view their own activities" });
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
                return Unauthorized(new { message = "Unauthenticated user" });
            if (userType != "organizer")
                return BadRequest(new { message = "Only organizers can view registration requests" });
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
                return Unauthorized(new { message = "Unauthenticated user" });
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
                return Unauthorized("Unauthenticated user");
            if (userType != "admin")
                return BadRequest("Only admin can ban activities");
            try
            {
                _repository.banActivity(activityId);
                return Ok(new { message = "Activity has been banned" });
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
                return Unauthorized("Unauthenticated user");
            if (userType != "admin")
                return BadRequest("Only admin can unban activities");
            try
            {
                _repository.UnbanActivity(activityId);
                return Ok(new { message = "Activity has been unbanned" });
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
                return Unauthorized("Unauthenticated user");
            if (userType != "admin")
                return BadRequest("Only admin can view all activity requests");
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

        [HttpGet("/checkRegisterStatus")]
        public string checkRegisterStatus(int activityId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return "Unauthenticated user";
            try
            {
                var message = _repository.checkRegisterStatus(username, activityId);
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet("/checkMyRegistionRequest")]
        public ActionResult<List<ActivityRegistrationRequest>> checkMyRegistionRequest()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "Unauthenticated user" });
            try
            {
                var requests = _repository.checkMyRegistrationRequests(username);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return NotFound(null);
            }
        }

        [HttpDelete("/cancelRegistration")]
        public IActionResult CancelRegistration(int activityId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Unauthenticated user");
            try
            {
                _repository.CancelRegistrationRequest(activityId, username);
                return Ok("Registration cancelled");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/getActivityByUserId")]
        public ActionResult<List<Activity>> GetActivityByUserId(int userId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "Unauthenticated user" });
            try
            {
                var activities = _repository.GetFavouriteByUserId(userId);
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("/cancleActivity")]
        public IActionResult CancelActivity(int activityId)
        {
            var username = HttpContext.Session.GetString("Username");
            var userType = HttpContext.Session.GetString("UserType");
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Unauthenticated user");
            if (userType != "organizer")
                return BadRequest("Only organizer and admin can cancel activity");
            try
            {
                _repository.cancelActivity(activityId);
                return Ok("Activity has been cancelled");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
