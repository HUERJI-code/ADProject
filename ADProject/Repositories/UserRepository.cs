using ADProject.Models;
using ADProject.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ADProject.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        private readonly Random _rand = new Random();
        private readonly SystemMessageRepository _systemMessageRepository;

        public UserRepository(AppDbContext context, SystemMessageRepository systemMessageRepository)
        {
            _context = context;
            _systemMessageRepository = systemMessageRepository;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users
                .Include(u => u.Profile)
                .Include(u => u.RegisteredActivities)
                .Include(u => u.Channels)
                .Include(u => u.ReceivedMessages)
                .ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users
                .Include(u => u.Profile)
                .Include(u => u.RegisteredActivities)
                .Include(u => u.Channels)
                .Include(u => u.ReceivedMessages)
                .FirstOrDefault(u => u.UserId == id);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            _context.Users.FirstOrDefault(u => u.Name == user.Name);
            if (user != null)
            {
                var CreateSystemMessageDto = new CreateSystemMessageDto
                {
                    Title = "create new user successfully",
                    Content = $"{user.Name} create new user account successfully",
                    ReceiverId = user.UserId 
                };
                _systemMessageRepository.Create(CreateSystemMessageDto);
            }
            else
            {
                Debug.WriteLine("User creation failed.");
            }
        }

        //public void Update(User user)
        //{
        //    _context.Users.Update(user);
        //    _context.SaveChanges();
        //}

        public string Update(UserUpdateDto userUpdateDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userUpdateDto.Email);
            if (user is null) return "No such user";
            if (userUpdateDto.Name == user.Name)
            {
                user.PasswordHash = userUpdateDto.NewPasswordHash;
                _context.SaveChanges();
                return "Password reset successfully";
            }
            else
            {
                return "Information mismatch";
            }

        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user is null) return;
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public bool ExistsByEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool ExistsByName(string name)
        {
            return _context.Users.Any(u => u.Name == name);
        }

        public void banUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user is null) return;
            user.status = "banned";
            _context.SaveChanges();
        }

        public void UnbanUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user is null) return;
            user.status = "active";
            _context.SaveChanges();
        }

        public int GetUserIdByUserName(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user.UserId;
        }

        public List<int> GetRandomUsers(int count = 5)
        {
            var allIds = _context.Users
                .Select(u => u.UserId)
                .ToList();

            return allIds
                .OrderBy(_ => _rand.Next())
                .Take(count)
                .ToList();
        }

        public String GenerizeInviteCode()
        {
            var code = GenerateRandomCode();
            while (_context.inviteCodes.Any(c => c.Code == code))
            {
                code = GenerateRandomCode();
            }
            var inviteCode = new InviteCode { Code = code, isUsed = false };
            _context.inviteCodes.Add(inviteCode);
            _context.SaveChanges();
            return code;
        }
        private string GenerateRandomCode(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_rand.Next(s.Length)]).ToArray());
        }

        public void UseInviteCode(string code)
        {
            var inviteCode = _context.inviteCodes.FirstOrDefault(c => c.Code == code);
            if (inviteCode != null && !inviteCode.isUsed)
            {
                inviteCode.isUsed = true;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Invalid or already used invite code.");
            }
        }
    }
}
