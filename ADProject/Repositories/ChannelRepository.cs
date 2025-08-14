using ADProject.Models;
using ADProject.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ADProject.Repositories
{
    public class ChannelRepository
    {
        private readonly AppDbContext _context;
        private readonly SystemMessageRepository _systemMessageRepository;

        public ChannelRepository(AppDbContext context, SystemMessageRepository systemMessageRepository)
        {
            _context = context;
            _systemMessageRepository = systemMessageRepository;
        }

        public void CreateChannel(string username, CreateChannelDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");
            if (user.Role != "organizer")
                throw new Exception("Only organizer can create channels");
            var existingChannel = _context.Channels.FirstOrDefault(c => c.Name == dto.Name);
            if (existingChannel != null)
                throw new Exception("A channel with the same name already exists, creation request denied");

            var tagEntities = _context.Tags
               .Where(t => dto.TagIds.Contains(t.TagId))
               .ToList();

            var channel = new Channel
            {
                Name = dto.Name,
                CreatedBy = user.UserId,
                Creator = user,
                status = "pending", // Initial status is pending
                Url = dto.Url ?? string.Empty, // Default to empty if no URL provided
                description = dto.Description ?? string.Empty, // Default to empty if no description provided
                Members = new List<User> { user }, // Default join the creator
                Tags = tagEntities, // Associated tags
            };

            _context.Channels.Add(channel);
            _context.SaveChanges(); // Get ChannelId

            var request = new ChannelRequest
            {
                ChannelId = channel.ChannelId,
                Status = "pending",
                channel = channel,
                OrganizerId = user.UserId,
                User = user,
                RequestedAt = DateTime.UtcNow,
            };
            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "new channel create request",
                Content = $"{user.Name} create new channel: {channel.Name} description: {channel.description}",
                ReceiverId = 1, // Assume 1 is the admin's UserId
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);

            _context.channelRequest.Add(request);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Save error: " + ex.InnerException?.Message);
                throw;
            }
        }

        public void UpdateChannel(UpdateChannelDto dto, string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");
            if (user.Role != "organizer")
                throw new Exception("Only organizer can update channels");
            var channel = _context.Channels
                .Include(c => c.Tags)
                .FirstOrDefault(c => c.ChannelId == dto.Id);
            if (channel == null)
                throw new Exception("Channel does not exist");
            if (channel.CreatedBy != user.UserId)
                throw new UnauthorizedAccessException("You do not have permission to update this channel");

            channel.Name = dto.Name;
            channel.description = dto.Description ?? string.Empty;
            channel.Url = dto.Url ?? string.Empty;

            if (dto.TagIds != null && dto.TagIds.Any())
            {
                var tagEntities = _context.Tags
                    .Where(t => dto.TagIds.Contains(t.TagId))
                    .ToList();
                channel.Tags.Clear();
                channel.Tags = tagEntities;
            }
            channel.status = "pending"; // Update status to pending, waiting for admin approval

            var request = new ChannelRequest
            {
                ChannelId = channel.ChannelId,
                Status = "pending",
                channel = channel,
                OrganizerId = user.UserId,
                User = user,
                RequestedAt = DateTime.UtcNow,
            };
            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "update channel information request",
                Content = $"{user.Name} update information for channel: {channel.Name} description: {channel.description}",
                ReceiverId = 1, // Assume 1 is the admin's UserId
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);
            _context.channelRequest.Add(request);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Save error: " + ex.InnerException?.Message);
                throw;
            }
        }

        public void ReviewChannelRequest(int requestId, string status)
        {
            var request = _context.channelRequest
                .Include(r => r.channel)
                .FirstOrDefault(r => r.Id == requestId);

            if (request == null)
                throw new Exception("Channel request record does not exist");

            if (status != "approved" && status != "rejected")
                throw new Exception("Invalid review status, must be approved or rejected");

            request.Status = status;
            request.ReviewedAt = DateTime.UtcNow;

            if (status == "approved" || status == "rejected" || status == "banned")
            {
                request.channel.status = status;
                var CreateSystemMessageDto = new CreateSystemMessageDto
                {
                    Title = "channel request result",
                    Content = $"channel: {request.channel.Name} request has been {status}, requester: {request.User.Name}",
                    ReceiverId = request.User.UserId,
                };
                _systemMessageRepository.Create(CreateSystemMessageDto);
            }

            _context.SaveChanges();
        }

        public void CreateChannelMessage(int channelId, PostChannelMessageDto dto, string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null || user.Role != "organizer")
                throw new UnauthorizedAccessException("Only organizer can post channel messages");
            var channel = _context.Channels.Find(channelId);
            if (channel == null)
                throw new Exception("Channel does not exist");
            if (channel.CreatedBy != user.UserId)
                throw new UnauthorizedAccessException("You do not have permission to post messages in this channel");
            if (channel.status != "approved")
                throw new Exception("Channel status does not allow posting messages");
            var message = new ChannelMessage
            {
                ChannelId = channelId,
                Title = dto.Title,
                Content = dto.Content,
                IsPinned = dto.IsPinned,
                IsVisible = dto.IsVisible,
                PostedById = user.UserId,
                PostedBy = user,
            };

            _context.ChannelMessages.Add(message);
            _context.SaveChanges();
        }

        public List<Channel> GetAllChannels()
        {
            return _context.Channels
                .Include(c => c.Creator)
                .Include(c => c.Members)
                .Include(c => c.Messages)
                .ToList();
        }

        public Channel GetChannelById(int channelId)
        {
            return _context.Channels
                .Include(c => c.Creator)
                .Include(c => c.Members)
                .Include(c => c.Messages)
                .FirstOrDefault(c => c.ChannelId == channelId);
        }

        public void JoinChannel(string username, int channelId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");

            var channel = _context.Channels
                .Include(c => c.Members)
                .FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("Channel does not exist");

            if (channel.status != "approved" && channel.status != "active")
                throw new Exception("Channel cannot be joined at this time");

            if (channel.Members.Any(m => m.UserId == user.UserId))
                throw new Exception("You are already in the channel");

            channel.Members.Add(user);
            _context.SaveChanges();
        }

        public void SubmitChannelReport(string username, int channelId, string content)
        {
            var channel = _context.Channels.FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("Report failed: Channel does not exist");
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");

            var report = new ChannelReport
            {
                ChannelId = channelId,
                Channel = channel,
                ReportedById = user.UserId,
                ReportedBy = user,
                Reason = content,
                Status = "pending", // Initial status is pending
                ReportedAt = DateTime.UtcNow,
                ReviewedAt = null
            };

            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "new channel report request",
                Content = $"{user.Name} report channel: {channel.Name} reason: {content}",
                ReceiverId = 1, // Assume 1 is the admin's UserId
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);

            _context.ChannelReports.Add(report);
            _context.SaveChanges();
        }

        public void ReviewChannelReport(int reportId, string newStatus)
        {
            var report = _context.ChannelReports.FirstOrDefault(r => r.Id == reportId);
            if (report == null)
                throw new Exception("Report record does not exist");

            if (string.IsNullOrWhiteSpace(newStatus))
                throw new Exception("Review status cannot be empty");

            report.Status = newStatus;
            report.ReviewedAt = DateTime.UtcNow;

            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "channel report result",
                Content = $"channel: {report.Channel.Name} report has been {newStatus}, reporter: {report.ReportedBy.Name}",
                ReceiverId = report.ReportedById,
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);

            _context.SaveChanges();
        }

        public List<ChannelMessage> GetChannelMessages(int channelId)
        {
            var channel = _context.Channels
                .Include(c => c.Messages)
                .ThenInclude(m => m.PostedBy)
                .FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("Channel does not exist");
            return channel.Messages
                .Where(m => m.IsVisible)
                .OrderByDescending(m => m.PostedAt)
                .ToList();
        }

        public List<User> GetChannelMembers(int channelId)
        {
            var channel = _context.Channels
                .Include(c => c.Members)
                .FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("Channel does not exist");
            return channel.Members.ToList();
        }

        public List<Channel> GetUserJoinedChannels(string username)
        {
            var user = _context.Users
                .Include(u => u.Channels)
                .ThenInclude(c => c.Creator)
                .FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");
            return user.Channels
                .Where(c => c.status == "approved" || c.status == "active")
                .ToList();
        }

        public List<Channel> GetOrganizerOwnedChannel(string username)
        {
            var channel = _context.Channels
                .Include(u => u.Creator)
                .ToList();
            var user = _context.Users.Where(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");
            var ownedChannel = channel.Where(c => c.Creator.Name == username);
            return ownedChannel.ToList();
        }

        public void banChannel(int channelId)
        {
            var channel = _context.Channels.FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("Channel does not exist");
            channel.status = "banned";
            _context.SaveChanges();
        }

        public void UnbanChannel(int channelId)
        {
            var channel = _context.Channels.FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("Channel does not exist");
            channel.status = "approved";
            _context.SaveChanges();
        }

        public List<ChannelReport> GetAllChannelReports()
        {
            return _context.ChannelReports
                .Include(r => r.ReportedBy)
                .Include(r => r.Channel)
                .ToList();
        }

        public List<ChannelRequest> GetAllChannelRequests()
        {
            return _context.channelRequest
                .Include(r => r.User)
                .Include(r => r.channel)
                .ToList();
        }

        public void quitChannel(string username, int channelId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("User does not exist");
            var channel = _context.Channels
                .Include(c => c.Members)
                .FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("Channel does not exist");
            if (!channel.Members.Any(m => m.UserId == user.UserId))
                throw new Exception("You are not in this channel");
            channel.Members.Remove(user);
            _context.SaveChanges();
        }

        public void cancelChannel(int channelId)
        {
            var channel = _context.Channels.FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("Channel does not exist");
            channel.status = "cancelled";
            _context.SaveChanges();
        }
    }
}
