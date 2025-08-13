using ADProject.Models;
using ADProject.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
                Title = "New Activity Create Request",
                Content = $"{user.Name} create new activity：{activity.Title} description： {activity.Description}",
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

            _context.ActivityRequest.Add(update);

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
                Title = "update activity information",
                Content = $"{activity.Creator.Name} update activity：{activity.Title} description： {activity.Description}",
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
                Title = "user sign for activity",
                Content = $"{username} sign for activity：{activity.Title}",
                ReceiverId = activity.Creator.UserId // 假设管理员ID为2
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);
        }

        public string checkRegisterStatus(string username, int activityId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                return ("用户不存在");

            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                return ("活动不存在");

            var existingRequest = _context.ActivityRegistrationRequests
                .FirstOrDefault(r => r.UserId == user.UserId && r.ActivityId == activityId);
            if (existingRequest != null)
                return ("已提交过申请或已注册该活动");
            else return ("未注册过");
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
                    var CreateSystemMessageDto = new CreateSystemMessageDto
                    {
                        Title = "your sign for activity has been approved",
                        Content = $"{request.User.Name} sign for：{activity.Title} has been approved",
                        ReceiverId = request.User.UserId
                    };
                    _systemMessageRepository.Create(CreateSystemMessageDto);
                }
            }

            if(decisionStatus == "rejected")
            {
                var CreateSystemMessageDto = new CreateSystemMessageDto
                {
                    Title = "your sign for activity has been rejected",
                    Content = $"{request.User.Name} sign for：{request.Activity.Title} has been rejected",
                    ReceiverId = request.User.UserId
                };
                _systemMessageRepository.Create(CreateSystemMessageDto);
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

        public List<Activity> GetRegisteredActivitiesByUserId(int userId)
        {
            var user = _context.Users
                .Include(u => u.RegisteredActivities)
                .FirstOrDefault(u => u.UserId == userId);
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

        public List<ActivityRegistrationRequest> checkMyRegistrationRequests(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");
            var requests = _context.ActivityRegistrationRequests
                .Include(r => r.Activity)
                .Where(r => r.UserId == user.UserId)
                .ToList();
            if (requests == null || requests.Count == 0)
                throw new Exception("没有注册申请");
            return requests;
        }

        public void CancelRegistrationRequest(int activityId, string username)
        {
            var activity = _context.Activities
                .Include(a => a.RegisteredUsers)
                //.Include(a => a.ActivityRegistrationRequests)
                .FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("活动不存在");

            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");

            // 1) 如果活动已报名用户中包含该用户，则移除
            // --- 取决于 RegisteredUsers 的类型与元素结构 ---
            // 情况A：RegisteredUsers 是 Users 集合
            if (activity.RegisteredUsers != null &&
                activity.RegisteredUsers.Any(u => u.UserId == user.UserId))
            {
                var regUser = activity.RegisteredUsers.First(u => u.UserId == user.UserId);
                activity.RegisteredUsers.Remove(regUser);
            }

            // 情况B：RegisteredUsers 是报名关联表(例如 ActivityUser / Registration)集合
            // if (activity.RegisteredUsers != null &&
            //     activity.RegisteredUsers.Any(r => r.UserId == user.UserId))
            // {
            //     var reg = activity.RegisteredUsers.First(r => r.UserId == user.UserId);
            //     activity.RegisteredUsers.Remove(reg);
            //     // 如果需要物理删除关联实体（而不是仅从导航集合移除），可额外调用：
            //     // _context.Remove(reg);
            // }

            // 2) 查找并删除该用户对该活动的注册申请
            var request = _context.ActivityRegistrationRequests
                .FirstOrDefault(r => r.UserId == user.UserId && r.ActivityId == activityId);
            if (request != null)
            {
                _context.ActivityRegistrationRequests.Remove(request);
            }

            // 3) 保存更改
            _context.SaveChanges();

            // 4) （可选）发送系统消息，仅当确实删除了申请时再发
            if (request != null)
            {
                var createSystemMessageDto = new CreateSystemMessageDto
                {
                    Title = "cancel registion request",
                    Content = $"{user.Name} cancel registion for {activity.Title} ",
                    ReceiverId = user.UserId
                };
                _systemMessageRepository.Create(createSystemMessageDto);
            }
        }

        public List<int> GetRandomRecommendation()
        {
            var today = DateTime.Today;
            // 1. 从数据库取出所有活动到内存
            var allActivities = _context.Activities.ToList();

            // 2. 过滤出 EndTime 在今天之后的活动
            var future = allActivities
                .Where(a =>
                {
                    // 解析 "yyyy/MM/dd HH:mm" 格式的 EndTime
                    if (DateTime.TryParseExact(
                            a.EndTime,
                            "yyyy/MM/dd HH:mm",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var dt))
                    {
                        return dt > today;
                    }
                    return false;
                });

            // 3. 随机打乱并取前 5 条
            var rand = new Random();
            var top5 = future
                .OrderBy(_ => rand.Next())
                .Take(5);

            // 4. 只返回 ActivityId 列表
            return top5
                .Select(a => a.ActivityId)
                .ToList();
        }

        public List<Activity> GetFavouriteByUserId(int userId)
        {
            var user = _context.Users
                .Include(u => u.favouriteActivities)
                .FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                throw new Exception("用户不存在");
            return user.favouriteActivities;


        }

        public void cancelActivity(int activityId)
        {
            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("活动不存在");
            activity.Status = "cancelled"; // 将活动状态设置为取消
            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "活动取消通知",
                Content = $"{activity.Title} 活动已被取消。",
                ReceiverId = activity.Creator.UserId // 通知活动创建者
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);
            _context.SaveChanges();
            foreach (var user in activity.RegisteredUsers)
            {
                var userMessage = new CreateSystemMessageDto
                {
                    Title = "activity has been cancelled",
                    Content = $"{activity.Title} has been cancelled。",
                    ReceiverId = user.UserId // 通知所有注册用户
                };
                _systemMessageRepository.Create(userMessage);
            }
            // 清除注册用户列表
            activity.RegisteredUsers.Clear();
            // 清除收藏用户列表
            activity.FavouritedByUsers.Clear();
            // 清除活动申请列表
            var requests = _context.ActivityRegistrationRequests
                .Where(r => r.ActivityId == activityId).ToList();
            foreach (var request in requests)
                {
                _context.ActivityRegistrationRequests.Remove(request);
            }
            // 清除活动请求列表
            var activityRequests = _context.ActivityRequest
                .Where(r => r.ActivityId == activityId).ToList();
            foreach (var activityRequest in activityRequests)
                {
                _context.ActivityRequest.Remove(activityRequest);
            }
            // 保存更改
            _context.SaveChanges();
        }
    }
}
