using ADProject.Models;
using ADProject.Services;
using Microsoft.EntityFrameworkCore;

namespace ADProject.Repositories
{
    public class ChannelRepository
    {
        private readonly AppDbContext _context;

        public ChannelRepository(AppDbContext context)
        {
            _context = context;
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

            if (status == "approved"|| status == "rejected" || status == "banned")
            {
                request.channel.status = status; // 更新频道状态为 approved
            }

            _context.SaveChanges();
        }

        public void CreateChannelMessage(int channelId, PostChannelMessageDto dto,string username)
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
                PostedAt = dto.PostedAt,
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

            _context.ChannelReports.Add(report);
            _context.SaveChanges();
        }


    }
}
