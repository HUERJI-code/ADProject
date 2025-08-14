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
                throw new Exception("User does not exist");

            var existingActivity = _context.Activities
                .FirstOrDefault(a => a.Title == dto.Title);

            if (existingActivity != null)
                throw new Exception("An activity with the same name already exists, please change the title.");

            var tagEntities = _context.Tags
                .Where(t => dto.TagIds.Contains(t.TagId))
                .ToList();

            Console.WriteLine($"Creating activity {dto.Title} by user {tagEntities.Count}");

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
                Url = dto.Url ?? string.Empty, // Default to empty if no URL provided
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
                Content = $"{user.Name} created a new activity: {activity.Title} description: {activity.Description}",
                ReceiverId = 1 // Assume admin ID is 2
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
                throw new Exception("Activity does not exist");

            activity.Title = dto.Title;
            activity.Description = dto.Description;
            activity.Location = dto.Location;
            activity.StartTime = dto.StartTime;
            activity.EndTime = dto.EndTime;
            activity.number = dto.number;
            activity.Url = dto.Url ?? string.Empty; // Default to empty if no URL provided
            activity.Status = "pending"; // Keep original status unless a new status is provided

            var update = new ActivityRequest
            {
                ActivityId = activity.ActivityId,
                ReviewedById = 1, // Assume admin ID is 2
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
                activity.Tags.Clear(); // Clear original tags
                activity.Tags = tagEntities;
            }

            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "Update activity information",
                Content = $"{activity.Creator.Name} updated activity: {activity.Title} description: {activity.Description}",
                ReceiverId = 1 // Assume admin ID is 2
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
                throw new Exception("Activity request record does not exist");

            request.Status = newStatus;
            request.ReviewedAt = DateTime.UtcNow;

            if (newStatus == "approved" || newStatus == "rejected")
            {
                request.Activity.Status = newStatus;
                var CreateSystemMessageDto = new CreateSystemMessageDto
                {
                    Title = "Activity request review result",
                    Content = $"{request.Activity.Title} has been {newStatus}",
                    ReceiverId = request.Activity.Creator.UserId
                };
                _systemMessageRepository.Create(CreateSystemMessageDto);
            }

            _context.SaveChanges();
        }

        public void RegisterForActivity(string username, int activityId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");

            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("Activity does not exist");

            var existingRequest = _context.ActivityRegistrationRequests
                .FirstOrDefault(r => r.UserId == user.UserId && r.ActivityId == activityId);
            if (existingRequest != null)
                throw new Exception("Application already submitted or registered for this activity");

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
                Title = "User signed up for activity",
                Content = $"{username} signed up for activity: {activity.Title}",
                ReceiverId = activity.Creator.UserId
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);
        }

        public string checkRegisterStatus(string username, int activityId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                return ("User does not exist");

            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                return ("Activity does not exist");

            var existingRequest = _context.ActivityRegistrationRequests
                .FirstOrDefault(r => r.UserId == user.UserId && r.ActivityId == activityId);
            if (existingRequest != null)
                return ("Application already submitted or registered for this activity");
            else return ("Not registered yet");
        }

        public void ReviewRegistrationRequest(int requestId, string decisionStatus)
        {
            var request = _context.ActivityRegistrationRequests
                .Include(r => r.User)
                .Include(r => r.Activity)
                .FirstOrDefault(r => r.Id == requestId);

            if (request == null)
                throw new Exception("Application record does not exist");

            if (decisionStatus != "approved" && decisionStatus != "rejected")
                throw new Exception("Invalid review status");

            request.Status = decisionStatus;
            request.ReviewedAt = DateTime.UtcNow;

            if (decisionStatus == "approved")
            {
                var activity = request.Activity;

                if (!activity.RegisteredUsers.Any(u => u.UserId == request.UserId))
                {
                    activity.RegisteredUsers.Add(request.User);
                    var CreateSystemMessageDto = new CreateSystemMessageDto
                    {
                        Title = "Your sign-up for activity has been approved",
                        Content = $"{request.User.Name} sign-up for: {activity.Title} has been approved",
                        ReceiverId = request.User.UserId
                    };
                    _systemMessageRepository.Create(CreateSystemMessageDto);
                }
            }

            if (decisionStatus == "rejected")
            {
                var CreateSystemMessageDto = new CreateSystemMessageDto
                {
                    Title = "Your sign-up for activity has been rejected",
                    Content = $"{request.User.Name} sign-up for: {request.Activity.Title} has been rejected",
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

            return titleMatches.Concat(tagMatches).Distinct().ToList();
        }

        public void AddToFavourites(string username, int activityId)
        {
            var user = _context.Users
                .Include(u => u.favouriteActivities)
                .FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");

            var activity = _context.Activities.Find(activityId);
            if (activity == null)
                throw new Exception("Activity does not exist");

            if (user.favouriteActivities.Any(a => a.ActivityId == activityId))
                throw new Exception("Activity is already in the favourites list");

            user.favouriteActivities.Add(activity);
            _context.SaveChanges();
        }

        public void RemoveFromFavourites(string username, int activityId)
        {
            var user = _context.Users
                .Include(u => u.favouriteActivities)
                .FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");

            var activity = user.favouriteActivities
                .FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("This activity is not in the favourites list");

            user.favouriteActivities.Remove(activity);
            _context.SaveChanges();
        }

        public List<Activity> GetFavouriteActivitiesByUsername(string username)
        {
            var user = _context.Users
                .Include(u => u.favouriteActivities)
                .FirstOrDefault(u => u.Name == username);

            if (user == null)
                throw new Exception("User does not exist");

            return user.favouriteActivities;
        }

        public List<Activity> GetRegisteredActivitiesByUsername(string username)
        {
            var user = _context.Users
                .Include(u => u.RegisteredActivities)
                .FirstOrDefault(u => u.Name == username);

            if (user == null)
                throw new Exception("User does not exist");

            return user.RegisteredActivities;
        }

        public List<Activity> GetRegisteredActivitiesByUserId(int userId)
        {
            var user = _context.Users
                .Include(u => u.RegisteredActivities)
                .FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                throw new Exception("User does not exist");
            return user.RegisteredActivities;
        }

        public List<Activity> GetLoginOrganizerActivities(string username)
        {
            var user = _context.Users
                .Include(u => u.RegisteredActivities)
                .Include(u => u.favouriteActivities)
                .FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");
            var activities = GetAll().Where(a => a.Creator.Name == username).ToList();
            return activities.Distinct().ToList();
        }

        public List<ActivityRegistrationRequest> GetOrganizerRegistrationRequests(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");
            var activityIdList = GetLoginOrganizerActivities(username)
                .Select(a => a.ActivityId)
                .ToList();
            var requests = _context.ActivityRegistrationRequests
                .Include(r => r.User)
                .Where(r => activityIdList.Contains(r.ActivityId) && r.Status == "pending")
                .ToList();
            if (requests == null || requests.Count == 0)
                throw new Exception("No pending registration requests");
            return requests;
        }

        public int GetActivityCountByActivityId(int activityId)
        {
            var activity = _context.Activities
                .Include(a => a.RegisteredUsers)
                .FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("Activity does not exist");
            return activity.RegisteredUsers.Count;
        }

        public void banActivity(int activityId)
        {
            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("Activity does not exist");
            activity.Status = "banned";
            _context.SaveChanges();
        }

        public void UnbanActivity(int activityId)
        {
            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("Activity does not exist");
            activity.Status = "approved";
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
                throw new Exception("User does not exist");
            var requests = _context.ActivityRegistrationRequests
                .Include(r => r.Activity)
                .Where(r => r.UserId == user.UserId)
                .ToList();
            if (requests == null || requests.Count == 0)
                throw new Exception("No registration applications");
            return requests;
        }

        public void CancelRegistrationRequest(int activityId, string username)
        {
            var activity = _context.Activities
                .Include(a => a.RegisteredUsers)
                .FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("Activity does not exist");

            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");

            if (activity.RegisteredUsers != null &&
                activity.RegisteredUsers.Any(u => u.UserId == user.UserId))
            {
                var regUser = activity.RegisteredUsers.First(u => u.UserId == user.UserId);
                activity.RegisteredUsers.Remove(regUser);
            }

            var request = _context.ActivityRegistrationRequests
                .FirstOrDefault(r => r.UserId == user.UserId && r.ActivityId == activityId);
            if (request != null)
            {
                _context.ActivityRegistrationRequests.Remove(request);
            }

            _context.SaveChanges();

            if (request != null)
            {
                var createSystemMessageDto = new CreateSystemMessageDto
                {
                    Title = "Cancel registration request",
                    Content = $"{user.Name} cancelled registration for {activity.Title}",
                    ReceiverId = user.UserId
                };
                _systemMessageRepository.Create(createSystemMessageDto);
            }
        }

        public List<int> GetRandomRecommendation()
        {
            var today = DateTime.Today;
            var allActivities = _context.Activities.ToList();

            var future = allActivities
                .Where(a =>
                {
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

            var rand = new Random();
            var top5 = future
                .OrderBy(_ => rand.Next())
                .Take(5);

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
                throw new Exception("User does not exist");
            return user.favouriteActivities;
        }

        public void cancelActivity(int activityId)
        {
            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity == null)
                throw new Exception("Activity does not exist");
            activity.Status = "cancelled";
            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "Activity has been cancelled",
                Content = $"{activity.Title} has been cancelled.",
                ReceiverId = activity.Creator.UserId
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);
            _context.SaveChanges();
            foreach (var user in activity.RegisteredUsers)
            {
                var userMessage = new CreateSystemMessageDto
                {
                    Title = "Activity has been cancelled",
                    Content = $"{activity.Title} has been cancelled.",
                    ReceiverId = user.UserId
                };
                _systemMessageRepository.Create(userMessage);
            }
            activity.RegisteredUsers.Clear();
            activity.FavouritedByUsers.Clear();
            var requests = _context.ActivityRegistrationRequests
                .Where(r => r.ActivityId == activityId).ToList();
            foreach (var request in requests)
            {
                _context.ActivityRegistrationRequests.Remove(request);
            }
            var activityRequests = _context.ActivityRequest
                .Where(r => r.ActivityId == activityId).ToList();
            foreach (var activityRequest in activityRequests)
            {
                _context.ActivityRequest.Remove(activityRequest);
            }
            _context.SaveChanges();
        }
    }
}
