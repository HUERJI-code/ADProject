using ADProject.Models;
using ADProject.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace ADProject.Repositories
{
    public class UserProfileRepository
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public UserProfileRepository(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<bool> RetrainModelAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:8000/retrain");
            return response.IsSuccessStatusCode;
        }

        public UserProfile GetProfileByUsername(string username)
        {
            var user = _context.Users
                .Include(u => u.Profile)
                .ThenInclude(p => p.Tags) // 加载标签
                .FirstOrDefault(u => u.Name == username);

            return user?.Profile;
        }

        public void UpsertProfile(string username, UpdateUserProfileDto dto)
        {
            var a = 0;

            var user = _context.Users
                .Include(u => u.Profile)
                .FirstOrDefault(u => u.Name == username);

            if (user == null)
                throw new Exception("用户不存在");

            var tagEntities = GetTagsByIds(dto.TagIds ?? new List<int>());

            if (user.Profile == null)
            {
                Console.WriteLine($" {username}");
                var newProfile = new UserProfile
                {
                    Age = dto.Age,
                    Gender = dto.Gender,
                    Tags = tagEntities,
                    UserId = user.UserId,
                    User = user, // 关联用户
                    url = dto.Url // 确保 URL 不为 null
                };

                _context.UserProfiles.Add(newProfile);
                user.Profile = newProfile;
                a = 1;
            }
            else
            {

                var existingProfile = user.Profile;
                existingProfile.Age = dto.Age;
                if(dto.Url != null) existingProfile.url = dto.Url;
                existingProfile.Gender = dto.Gender;
                Console.WriteLine($"更新用户 {username} 的个人资料");
                existingProfile.Tags.Clear(); // 清除原有绑定
                existingProfile.Tags = tagEntities;
            }

            try
            {
                _context.SaveChanges();
                if(a == 1)
                {
                    Console.WriteLine($"创建用户 {username} 的个人资料");
                    RetrainModelAsync().Wait();
                }
                else
                {
                    Console.WriteLine($"更新用户 {username} 的个人资料");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }

        }


        public List<Tag> GetTagsByIds(List<int> tagIds)
        {
            return _context.Tags
                .Where(t => tagIds.Contains(t.TagId))
                .ToList();
        }
    }
}
