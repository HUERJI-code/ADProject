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

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.RegisteredActivities)
                .Include(u => u.Channels)
                .Include(u => u.ReceivedMessages)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.RegisteredActivities)
                .Include(u => u.Channels)
                .Include(u => u.ReceivedMessages)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
