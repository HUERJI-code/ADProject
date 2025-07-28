using ADProject.Models;
using ADProject.Services;

namespace ADProject.Repositories
{
    public class ActivityRepository
    {
        private readonly AppDbContext _context;

        public ActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateActivityWithRegistration(string username, CreateActivityDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");

            var existingActivity = _context.Activities
                .FirstOrDefault(a => a.Title == dto.Title);

            if (existingActivity != null)
                throw new Exception("已有同名活动，请更换标题。");

            var tagEntities = _context.Tags
                .Where(t => dto.TagIds.Contains(t.TagId))
                .ToList();

            Console.WriteLine($"创建活动 {dto.Title} 的用户 {tagEntities.Count}");

            var activity = new Activity
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                CreatedBy = user.UserId,
                Creator = user,
                Tags = tagEntities
            };

            _context.Activities.Add(activity);
            _context.SaveChanges();

            var admin = _context.Users.FirstOrDefault(u => u.UserId == 2);
            var registration = new ActivityRequest
            {
                ActivityId = activity.ActivityId,
                ReviewedById = 2,
                Status = "pending",
                RequestedAt = DateTime.UtcNow,
                ReviewedAt = DateTime.UtcNow,
                requestType = "createActivity",
                ReviewedBy = admin,
            };

            _context.ActivityRequest.Add(registration);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
            }

        }


        public Activity GetById(int id) => _context.Activities.Find(id);
        public List<Activity> GetAll() => _context.Activities.ToList();


        public void UpdateActivity(int activityId, UpdateActivityDto dto)
        {
            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("活动不存在");

            activity.Title = dto.Title;
            activity.Description = dto.Description;
            activity.Location = dto.Location;
            activity.StartTime = dto.StartTime;
            activity.EndTime = dto.EndTime;

            if (dto.TagIds != null)
            {
                var tagEntities = _context.Tags
                    .Where(t => dto.TagIds.Contains(t.TagId))
                    .ToList();
                activity.Tags.Clear(); // 清除原有绑定
                activity.Tags = tagEntities;
            }

            _context.SaveChanges();
        }

    }
}
