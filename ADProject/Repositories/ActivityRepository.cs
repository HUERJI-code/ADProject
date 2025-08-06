using ADProject.Models;
using ADProject.Services;
using Microsoft.EntityFrameworkCore;

namespace ADProject.Repositories
{
    public class ActivityRepository
    {
        private readonly AppDbContext _context;
        private readonly SystemMessageRepository _systemMessageRepository;

        public ActivityRepository(AppDbContext context, SystemMessageRepository systemMessageRepository)
        {
            _context = context;
            _systemMessageRepository = systemMessageRepository;
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
                number = dto.number,
                Url = dto.Url ?? string.Empty, // 如果没有链接则默认为空
                Tags = tagEntities
            };

            _context.Activities.Add(activity);
            _context.SaveChanges();

            var admin = _context.Users.FirstOrDefault(u => u.UserId == 2);
            var registration = new ActivityRequest
            {
                ActivityId = activity.ActivityId,
                ReviewedById = 1,
                Status = "pending",
                RequestedAt = DateTime.UtcNow,
                requestType = "createActivity",
                ReviewedBy = admin,
            };

            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "新活动申请",
                Content = $"{user.Name} 创建了新活动：{activity.Title} 描述为： {activity.Description}",
                ReceiverId = 1 // 假设管理员ID为2
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);

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
            activity.number = dto.number;
            activity.Url = dto.Url ?? string.Empty; // 如果没有链接则默认为空
            activity.Status = "pending"; // 保持原有状态，除非提供了新的状态

            var update = new ActivityRequest
            {
                ActivityId = activity.ActivityId,
                ReviewedById = 1, // 假设管理员ID为2
                Status = "pending",
                RequestedAt = DateTime.UtcNow,
                requestType = "updateActivity",
                ReviewedBy = _context.Users.FirstOrDefault(u => u.UserId == 1)
            };

            if (dto.TagIds != null)
            {
                var tagEntities = _context.Tags
                    .Where(t => dto.TagIds.Contains(t.TagId))
                    .ToList();
                activity.Tags.Clear(); // 清除原有绑定
                activity.Tags = tagEntities;
            }

            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "活动信息修改",
                Content = $"{activity.Creator.Name} 创建了新活动：{activity.Title} 描述为： {activity.Description}",
                ReceiverId = 1 // 假设管理员ID为2
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);

            _context.SaveChanges();
        }

        public void ApproveActivityRequest(int requestId, string newStatus)
        {
            var request = _context.ActivityRequest
                .Include(r => r.Activity)
                .FirstOrDefault(r => r.Id == requestId);

            if (request == null)
                throw new Exception("活动申请记录不存在");

            // 更新申请状态
            request.Status = newStatus;
            request.ReviewedAt = DateTime.UtcNow;

            // 如果是审批通过或拒绝，则更新对应活动状态
            if (newStatus == "approved" || newStatus == "rejected")
            {
                request.Activity.Status = newStatus;
            }

            _context.SaveChanges();
        }

        public void RegisterForActivity(string username, int activityId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");

            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("活动不存在");

            var existingRequest = _context.ActivityRegistrationRequests
                .FirstOrDefault(r => r.UserId == user.UserId && r.ActivityId == activityId);
            if (existingRequest != null)
                throw new Exception("已提交过申请或已注册该活动");

            var request = new ActivityRegistrationRequest
            {
                UserId = user.UserId,
                User = user,
                ActivityId = activityId,
                Activity = activity,
                Status = "pending",
                RequestedAt = DateTime.UtcNow
            };

            _context.ActivityRegistrationRequests.Add(request);
            _context.SaveChanges();

            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "新用户申请活动",
                Content = $"{username} 申请注册活动：{activity.Title}",
                ReceiverId = activity.Creator.UserId // 假设管理员ID为2
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);
        }

        public void ReviewRegistrationRequest(int requestId, string decisionStatus)
        {
            var request = _context.ActivityRegistrationRequests
                .Include(r => r.User)
                .Include(r => r.Activity)
                .FirstOrDefault(r => r.Id == requestId);

            if (request == null)
                throw new Exception("申请记录不存在");

            if (decisionStatus != "approved" && decisionStatus != "rejected")
                throw new Exception("审核状态无效");

            request.Status = decisionStatus;
            request.ReviewedAt = DateTime.UtcNow;

            if (decisionStatus == "approved")
            {
                var activity = request.Activity;

                // 避免重复添加
                if (!activity.RegisteredUsers.Any(u => u.UserId == request.UserId))
                {
                    activity.RegisteredUsers.Add(request.User);
                }
            }

            _context.SaveChanges();
        }

        public List<Activity> SearchActivitiesByKeyword(string keyword)
        {
            var titleMatches = _context.Activities
                .Include(a => a.Tags)
                .Where(a => a.Title.Contains(keyword))
                .ToList();

            var tagMatches = _context.Activities
                .Include(a => a.Tags)
                .Where(a => a.Tags.Any(t => t.Name.Contains(keyword)))
                .ToList();

            // 合并并去重
            var allMatches = titleMatches
                .Concat(tagMatches)
                .Distinct()
                .ToList();

            return allMatches;
        }


        public void AddToFavourites(string username, int activityId)
        {
            var user = _context.Users
                .Include(u => u.favouriteActivities)
                .FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");

            var activity = _context.Activities.Find(activityId);
            if (activity == null)
                throw new Exception("活动不存在");

            if (user.favouriteActivities.Any(a => a.ActivityId == activityId))
                throw new Exception("活动已在收藏列表");

            user.favouriteActivities.Add(activity);
            _context.SaveChanges();
        }

        public void RemoveFromFavourites(string username, int activityId)
        {
            var user = _context.Users
                .Include(u => u.favouriteActivities)
                .FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");

            var activity = user.favouriteActivities
                .FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("该活动不在收藏列表");

            user.favouriteActivities.Remove(activity);
            _context.SaveChanges();
        }

        public List<Activity> GetFavouriteActivitiesByUsername(string username)
        {
            var user = _context.Users
                .Include(u => u.favouriteActivities)
                .FirstOrDefault(u => u.Name == username);

            if (user == null)
                throw new Exception("用户不存在");

            return user.favouriteActivities;
        }

        public List<Activity> GetRegisteredActivitiesByUsername(string username)
        {
            var user = _context.Users
                .Include(u => u.RegisteredActivities)
                .FirstOrDefault(u => u.Name == username);

            if (user == null)
                throw new Exception("用户不存在");

            return user.RegisteredActivities;
        }

        public List<Activity> GetLoginOrganizerActivities(string username)
        {
            var user = _context.Users
                .Include(u => u.RegisteredActivities)
                .Include(u => u.favouriteActivities)
                .FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");
            var activities = GetAll().Where(a => a.Creator.Name == username).ToList();
            return activities.Distinct().ToList();

        }

        public List<ActivityRegistrationRequest> GetOrganizerRegistrationRequests(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");
            var activityIdList = GetLoginOrganizerActivities(username)
                .Select(a => a.ActivityId)
                .ToList();
            var requests = _context.ActivityRegistrationRequests
                .Include(r => r.User)
                .Where(r => activityIdList.Contains(r.ActivityId) && r.Status == "pending")
                .ToList();
            if (requests == null || requests.Count == 0)
                throw new Exception("没有待处理的注册申请");
            return requests;

        }

        public int GetActivityCountByActivityId(int activityId)
        {
            var activity = _context.Activities
                .Include(a => a.RegisteredUsers)
                .FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("活动不存在");
            return activity.RegisteredUsers.Count;
        }

        public void banActivity(int activityId)
        {
            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("活动不存在");
            activity.Status = "banned"; // 将活动状态设置为取消
            _context.SaveChanges();
        }

        public void UnbanActivity(int activityId)
        {
            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("活动不存在");
            activity.Status = "approved"; // 将活动状态设置为已批准
            _context.SaveChanges();
        }

        public List<ActivityRequest> GetAllActivityRequests()
        {
            return _context.ActivityRequest
                .Include(r => r.Activity)
                .Include(r => r.ReviewedBy)
                .ToList();
        }
    }
}
