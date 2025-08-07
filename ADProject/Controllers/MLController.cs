using ADProject.Models;
using ADProject.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
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

        public MLController(UserRepository userRepository, IHttpClientFactory httpClientFactory, ActivityRepository repository)
        {
            _userRepository = userRepository;
            _httpClient = httpClientFactory.CreateClient();
            _activityRepository = repository;
        }

        [HttpGet("/getRecommendations")]
        public async Task<ActionResult<List<int>>> GetRecommendations()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("User not logged in.");
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
                top_k = 3
            };
            var response = await _httpClient.PostAsJsonAsync(
            "http://127.0.0.1:8000/recommend/",
            payload
        );

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to fetch recommendations");
            }

            // 反序列化返回的 JSON
            var result = await response.Content.ReadFromJsonAsync<RecommendResponse>();

            if (result?.RecommendedActivityIds == null)
            {
                return BadRequest("Invalid response from recommendation service");
            }

            // 只返回 List<int>
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
            // 构造要发给 FastAPI 的 JSON
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
                top_k = 3
            };
            if( user == null)
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
            // 调用 FastAPI
            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:8000/similar-users",
                payload
            );

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch similar users");

            // 反序列化 FastAPI 的原始返回
            var serviceResult = await response.Content
                .ReadFromJsonAsync<SimilarUsersServiceResponse>();

            if (serviceResult?.SimilarUsers == null)
                return BadRequest("Invalid response from similar-users service");

            // 提取纯 int 列表
            var ids = serviceResult.SimilarUsers
                .Select(x => x.UserId)
                .ToList();

            // 返回最终格式
            return Ok(new SimilarUsersListResponse { SimilarUsers = ids });
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

        // FastAPI 那边数组里，每个元素形如 { "user_id": 11 }
        private class SimilarUserItem
        {
            [JsonPropertyName("user_id")]
            public int UserId { get; set; }
        }


    }
}
