using ManagementService.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace ManagementService
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        // user repository ctor
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // get all users
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // get user by car id
        public async Task<User?> GetUserByCarIdAsync(Guid carId)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.CarId == carId);
        }

        // get user by user id
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        // add new user to DB
        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // update user info to db
        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // delete user from db
        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"Could not find user with id {userId}");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}