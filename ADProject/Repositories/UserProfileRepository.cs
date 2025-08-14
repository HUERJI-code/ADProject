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
                .ThenInclude(p => p.Tags) // Load tags
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
                throw new Exception("User does not exist");

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
                    User = user, // Associate with user
                    url = dto.Url // Ensure URL is not null
                };

                _context.UserProfiles.Add(newProfile);
                user.Profile = newProfile;
                a = 1;
            }
            else
            {
                var existingProfile = user.Profile;
                existingProfile.Age = dto.Age;
                if (dto.Url != null) existingProfile.url = dto.Url;
                existingProfile.Gender = dto.Gender;
                Console.WriteLine($"Updating profile for user {username}");
                existingProfile.Tags.Clear(); // Clear existing bindings
                existingProfile.Tags = tagEntities;
            }

            try
            {
                _context.SaveChanges();
                if (a == 1)
                {
                    Console.WriteLine($"Created profile for user {username}");
                    RetrainModelAsync().Wait();
                }
                else
                {
                    Console.WriteLine($"Updated profile for user {username}");
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
