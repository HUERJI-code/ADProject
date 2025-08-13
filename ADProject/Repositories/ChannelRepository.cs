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
                throw new Exception("用户不存在");
            if (user.Role != "organizer")
                throw new Exception("只有 organizer 可以创建频道");
            var existingChannel = _context.Channels.FirstOrDefault(c => c.Name == dto.Name);
            if (existingChannel != null)
                throw new Exception("已存在同名频道，创建请求被拒绝");

            var tagEntities = _context.Tags
               .Where(t => dto.TagIds.Contains(t.TagId))
               .ToList();

            var channel = new Channel
            {
                Name = dto.Name,
                CreatedBy = user.UserId,
                Creator = user,
                status = "pending", // 初始状态为 pending
                Url = dto.Url ?? string.Empty, // 如果没有链接则默认为空
                description = dto.Description ?? string.Empty, // 如果没有描述则默认为空
                Members = new List<User> { user }, // 默认加入创建者
                Tags = tagEntities, // 关联标签
            };

            _context.Channels.Add(channel);
            _context.SaveChanges(); // 获取 ChannelId

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
                Content = $"{user.Name} create new channel：{channel.Name} description： {channel.description}",
                ReceiverId = 1, // 假设 1 是管理员的 UserId
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);

            _context.channelRequest.Add(request); // 如果你有单独的 ChannelRequest 表可以替换这里
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
                throw new Exception("用户不存在");
            if (user.Role != "organizer")
                throw new Exception("只有 organizer 可以更新频道");
            var channel = _context.Channels
                .Include(c => c.Tags) // 确保包含 Tags
                .FirstOrDefault(c => c.ChannelId == dto.Id);
            if (channel == null)
                throw new Exception("频道不存在");
            if (channel.CreatedBy != user.UserId)
                throw new UnauthorizedAccessException("您没有权限更新此频道");
            // 更新频道信息
            channel.Name = dto.Name;
            channel.description = dto.Description ?? string.Empty; // 如果没有描述则默认为空
            channel.Url = dto.Url ?? string.Empty; // 如果没有链接则默认为空
            // 更新标签
            if (dto.TagIds != null && dto.TagIds.Any())
            {
                var tagEntities = _context.Tags
                    .Where(t => dto.TagIds.Contains(t.TagId))
                    .ToList();
                channel.Tags.Clear(); // 清空现有标签
                channel.Tags = tagEntities; // 更新标签列表
            }
            channel.status = "pending"; // 更新状态为 pending，等待管理员审核
            // 创建频道更新请求
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
                Content = $"{user.Name} update information for channel：{channel.Name} description： {channel.description}",
                ReceiverId = 1, // 假设 1 是管理员的 UserId
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);
            _context.channelRequest.Add(request); // 如果你有单独的 ChannelRequest 表可以替换这里
            // 保存更改
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
                throw new Exception("频道请求记录不存在");

            if (status != "approved" && status != "rejected")
                throw new Exception("审核状态无效，只能为 approved 或 rejected");

            request.Status = status;
            request.ReviewedAt = DateTime.UtcNow;

            if (status == "approved" || status == "rejected" || status == "banned")
            {
                request.channel.status = status; // 更新频道状态为 approved
                var CreateSystemMessageDto = new CreateSystemMessageDto
                {
                    Title = "channel request result",
                    Content = $"channel：{request.channel.Name} request has been {status}，requester：{request.User.Name}",
                    ReceiverId = request.User.UserId, // 通知申请人
                };
            }

            _context.SaveChanges();
        }

        public void CreateChannelMessage(int channelId, PostChannelMessageDto dto, string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null || user.Role != "organizer")
                throw new UnauthorizedAccessException("仅 organizer 可发布频道消息");
            var channel = _context.Channels.Find(channelId);
            if (channel == null)
                throw new Exception("频道不存在");
            if (channel.CreatedBy != user.UserId)
                throw new UnauthorizedAccessException("您没有权限在此频道发布消息");
            if (channel.status != "approved")
                throw new Exception("频道状态不允许发布消息");
            var message = new ChannelMessage
            {
                ChannelId = channelId,
                Title = dto.Title,
                Content = dto.Content,
                //PostedAt = dto.PostedAt,
                IsPinned = dto.IsPinned,
                IsVisible = dto.IsVisible,
                PostedById = user.UserId, // 发布人ID
                PostedBy = user, // 发布人对象
            };

            _context.ChannelMessages.Add(message);
            _context.SaveChanges();
        }
        public List<Channel> GetAllChannels()
        {
            return _context.Channels
                .Include(c => c.Creator)
                .Include(c => c.Members)
                .Include(c => c.Messages) // 包含消息
                .ToList();
        }
        public Channel GetChannelById(int channelId)
        {
            return _context.Channels
                .Include(c => c.Creator)
                .Include(c => c.Members)
                .Include(c => c.Messages) // 包含消息
                .FirstOrDefault(c => c.ChannelId == channelId);
        }

        public void JoinChannel(string username, int channelId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");

            var channel = _context.Channels
                .Include(c => c.Members)
                .FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("频道不存在");

            if (channel.status != "approved" && channel.status != "active")
                throw new Exception("频道暂时无法加入");

            if (channel.Members.Any(m => m.UserId == user.UserId))
                throw new Exception("您已在频道中");

            channel.Members.Add(user);
            _context.SaveChanges();
        }

        public void SubmitChannelReport(string username, int channelId, string content)
        {
            var channel = _context.Channels.FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("举报失败：频道不存在");
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");

            var report = new ChannelReport
            {
                ChannelId = channelId,
                Channel = channel,
                ReportedById = user.UserId,
                ReportedBy = user,
                Reason = content,
                Status = "pending", // 初始状态为 pending
                ReportedAt = DateTime.UtcNow,
                ReviewedAt = null // 初始时未审核
            };

            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "new channel report request",
                Content = $"{user.Name} report cahnnel：{channel.Name} reason： {content}",
                ReceiverId = 1, // 假设 1 是管理员的 UserId
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);

            _context.ChannelReports.Add(report);
            _context.SaveChanges();
        }

        public void ReviewChannelReport(int reportId, string newStatus)
        {
            var report = _context.ChannelReports.FirstOrDefault(r => r.Id == reportId);
            if (report == null)
                throw new Exception("举报记录不存在");

            if (string.IsNullOrWhiteSpace(newStatus))
                throw new Exception("审批状态不能为空");

            report.Status = newStatus; // 例如："resolved", "rejected"
            report.ReviewedAt = DateTime.UtcNow;

            var CreateSystemMessageDto = new CreateSystemMessageDto
            {
                Title = "channel report result",
                Content = $"channel：{report.Channel.Name} report has been {newStatus}，reporter：{report.ReportedBy.Name}",
                ReceiverId = report.ReportedById, // 通知申请人
            };
            _systemMessageRepository.Create(CreateSystemMessageDto);

            _context.SaveChanges();
        }

        public List<ChannelMessage> GetChannelMessages(int channelId)
        {
            var channel = _context.Channels
                .Include(c => c.Messages)
                .ThenInclude(m => m.PostedBy) // 包含发布者信息
                .FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("频道不存在");
            return channel.Messages
                .Where(m => m.IsVisible) // 只返回可见的消息
                .OrderByDescending(m => m.PostedAt) // 按时间降序排列
                .ToList();
        }

        public List<User> GetChannelMembers(int channelId)
        {
            var channel = _context.Channels
                .Include(c => c.Members)
                .FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("频道不存在");
            return channel.Members.ToList();

        }

        public List<Channel> GetUserJoinedChannels(string username)
        {
            var user = _context.Users
                .Include(u => u.Channels)
                .ThenInclude(c => c.Creator) // 包含频道创建者信息
                .FirstOrDefault(u => u.Name == username); // 替换为实际用户名
            if (user == null)
                throw new Exception("用户不存在");
            return user.Channels
                .Where(c => c.status == "approved" || c.status == "active") // 只返回已批准或激活的频道
                .ToList();
        }

        public List<Channel> GetOrganizerOwnedChannel(string username)
        {
            var channel = _context.Channels
                .Include(u => u.Creator)
                .ToList();
            var user = _context.Users.Where(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");
            var ownedChannel = channel.Where(c => c.Creator.Name == username);
            return ownedChannel.ToList();
        }

        public void banChannel(int channelId)
        {
            var channel = _context.Channels.FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("频道不存在");
            channel.status = "banned"; // 将频道状态设置为 banned
            _context.SaveChanges();

        }

        public void UnbanChannel(int channelId)
        {
            var channel = _context.Channels.FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("频道不存在");
            channel.status = "approved"; // 将频道状态设置为 approved
            _context.SaveChanges();
        }

        public List<ChannelReport> GetAllChannelReports()
        {
            return _context.ChannelReports
                .Include(r => r.ReportedBy) // 包含举报者信息
                .Include(r => r.Channel) // 包含被举报的频道信息
                .ToList();
        }

        public List<ChannelRequest> GetAllChannelRequests()
        {
            return _context.channelRequest
                .Include(r => r.User) // 包含申请者信息
                .Include(r => r.channel) // 包含申请的频道信息
                .ToList();
        }

        public void quitChannel(string username, int channelId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                throw new Exception("用户不存在");
            var channel = _context.Channels
                .Include(c => c.Members)
                .FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("频道不存在");
            if (!channel.Members.Any(m => m.UserId == user.UserId))
                throw new Exception("您不在此频道中");
            channel.Members.Remove(user);
            _context.SaveChanges();
        }

        public void cancelChannel(int channelId)
        {
            var channel = _context.Channels.FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
                throw new Exception("频道不存在");
            channel.status = "cancelled"; // 将频道状态设置为 cancelled
            _context.SaveChanges();
        }
    }
}
