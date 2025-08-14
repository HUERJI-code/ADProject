using ADProject.Services;
using ADProject.Models;

namespace ADProject.Repositories
{
    public class SystemMessageRepository
    {
        private readonly AppDbContext _context;

        public SystemMessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<SystemMessage> GetAll()
        {
            return _context.SystemMessages.ToList();
        }

        public SystemMessage GetById(int id)
        {
            return _context.SystemMessages.FirstOrDefault(m => m.Id == id);
        }

        public List<SystemMessage> GetByUserName(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
                return null;

            return _context.SystemMessages
                .Where(m => m.Receiver.Name == username)
                .ToList();
        }

        public void Create(CreateSystemMessageDto createSystemMessageDto)
        {
            var receiver = _context.Users.FirstOrDefault(u => u.UserId == createSystemMessageDto.ReceiverId);
            if (receiver == null)
                throw new Exception("Receiver does not exist");
            var systemMessage = new SystemMessage
            {
                Title = createSystemMessageDto.Title,
                Content = createSystemMessageDto.Content,
                Receiver = receiver,
                IsRead = false, // Default unread
                ReceiverId = receiver.UserId,
                SentAt = DateTime.UtcNow
            };
            _context.SystemMessages.Add(systemMessage);
            _context.SaveChanges();
        }

        public void MarkAsRead(int id)
        {
            var message = _context.SystemMessages.FirstOrDefault(m => m.Id == id);
            if (message == null)
                throw new Exception("Message does not exist");
            message.IsRead = true;
            _context.SaveChanges();
        }
    }
}
