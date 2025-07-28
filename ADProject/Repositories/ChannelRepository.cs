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

            var channel = new Channel
            {
                Name = dto.Name,
                CreatedBy = user.UserId,
                Creator = user,
                status = "pending", // 初始状态为 pending
                description = dto.Description ?? string.Empty, // 如果没有描述则默认为空
                Members = new List<User> { user }, // 默认加入创建者
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

    }
}
