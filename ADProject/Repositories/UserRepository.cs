using ADProject.Models;
using ADProject.Services;
using Microsoft.EntityFrameworkCore;

namespace ADProject.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
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
        }

        //public void Update(User user)
        //{
        //    _context.Users.Update(user);
        //    _context.SaveChanges();
        //}

        public string Update(UserUpdateDto userUpdateDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userUpdateDto.Email);
            if (user is null) return "没有这个用户";
            if(userUpdateDto.Name == user.Name)
            {
                user.PasswordHash = userUpdateDto.NewPasswordHash;
                _context.SaveChanges();
                return "密码重置成功";
            }
            else
            {
                return "信息匹配错误";
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
    }
}
