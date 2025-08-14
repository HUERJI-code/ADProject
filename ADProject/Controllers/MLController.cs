using ADProject.Models;
using ADProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace ADProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MLController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly HttpClient _httpClient;
        private readonly ActivityRepository _activityRepository;
        private readonly TagRepository _tagRepository;

        public MLController(UserRepository userRepository, IHttpClientFactory httpClientFactory, ActivityRepository repository, TagRepository tagRepository)
        {
            _userRepository = userRepository;
            _httpClient = httpClientFactory.CreateClient();
            _activityRepository = repository;
            _tagRepository = tagRepository;
        }

        [HttpGet("/getRecommendations")]
        public async Task<ActionResult<List<int>>> GetRecommendations()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return Ok(_activityRepository.GetRandomRecommendation());
            }
            var userId = _userRepository.GetUserIdByUserName(username);
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (user.status == "banned")
            {
                return Forbid("User is banned.");
            }
            if (user.Profile == null)
            {
                return Ok(_activityRepository.GetRandomRecommendation());
            }
            var payload = new
            {
                user_id = userId,
                top_k = 5
            };
            var response = await _httpClient.PostAsJsonAsync(
                //"http://127.0.0.1:8000/recommend/",
                "https://adprojectml-c6g5egcpbkfkfqcn.southeastasia-01.azurewebsites.net/recommendActivity/",
                payload
            );

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to fetch recommendations");
            }

            // Deserialize JSON response
            var result = await response.Content.ReadFromJsonAsync<RecommendResponse>();

            if (result?.RecommendedActivityIds == null)
            {
                return BadRequest("Invalid response from recommendation service");
            }

            // Only return List<int>
            return Ok(result.RecommendedActivityIds);
        }

        private class RecommendResponse
        {
            [JsonPropertyName("recommended_activity_ids")]
            public List<int> RecommendedActivityIds { get; set; }
        }

        [HttpPost("/similar-users")]
        public async Task<ActionResult<SimilarUsersListResponse>> SimilarUsers()
        {
            // Construct JSON to send to FastAPI
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("User not logged in.");
            }
            var userId = _userRepository.GetUserIdByUserName(username);
            var user = _userRepository.GetById(userId);
            var payload = new
            {
                user_id = userId,
                top_k = 5
            };
            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (user.status == "banned")
            {
                return Forbid("User is banned.");
            }
            if (user.Profile == null)
            {
                return Ok(_userRepository.GetRandomUsers());
            }
            // Call FastAPI
            var response = await _httpClient.PostAsJsonAsync(
                //"http://localhost:8000/similar-users",
                "https://adprojectml-c6g5egcpbkfkfqcn.southeastasia-01.azurewebsites.net/recommendUser/",
                payload
            );

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch similar users");

            // Deserialize FastAPI response
            var serviceResult = await response.Content
                .ReadFromJsonAsync<SimilarUsersServiceResponse>();

            if (serviceResult?.SimilarUsers == null)
                return BadRequest("Invalid response from similar-users service");

            // Extract list of user IDs
            var ids = serviceResult.SimilarUsers
                .Select(x => x.UserId)
                .ToList();

            // Return final result
            return Ok(ids);
        }

        public class SimilarUsersListResponse
        {
            [JsonPropertyName("similar_users")]
            public List<int> SimilarUsers { get; set; }
        }

        private class SimilarUsersServiceResponse
        {
            [JsonPropertyName("user_id")]
            public int UserId { get; set; }

            [JsonPropertyName("similar_users")]
            public List<SimilarUserItem> SimilarUsers { get; set; }
        }

        // Each element from FastAPI is like { "user_id": 11 }
        private class SimilarUserItem
        {
            [JsonPropertyName("user_id")]
            public int UserId { get; set; }
        }

        [HttpGet("/retrainModel")]
        public async Task<IActionResult> RetrainModel()
        {
            // Call FastAPI retrain endpoint
            var response = await _httpClient.GetAsync("https://adprojectml-c6g5egcpbkfkfqcn.southeastasia-01.azurewebsites.net/TrainRecommender/");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to retrain model");
            }
            // Return success message
            return Ok("Model retrained successfully");
        }

        [HttpPost("predict-tags")]
        public async Task<IActionResult> PredictTags([FromBody] PredictTagsRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.title))
            {
                return BadRequest("Invalid request data");
            }
            // Call FastAPI predict_tags endpoint
            var response = await _httpClient.PostAsJsonAsync(
                "https://adprojectml-c6g5egcpbkfkfqcn.southeastasia-01.azurewebsites.net/predictTags/",
                request
            );
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to predict tags");
            }
            // Deserialize JSON response
            var result = await response.Content.ReadFromJsonAsync<PredictTagsResponse>();
            if (result?.Tags == null)
            {
                return BadRequest("Invalid response from prediction service");
            }
            var tagIds = _tagRepository.AddTagIfNotExists(result.Tags);
            return Ok(tagIds);
        }

        public class PredictTagsRequest
        {
            [JsonPropertyName("title")]
            public string title { get; set; }

            [JsonPropertyName("description")]
            public string description { get; set; }
        }

        public class PredictTagsResponse
        {
            [JsonPropertyName("predicted_tags")]
            public List<string> Tags { get; set; }
        }
    }
}
