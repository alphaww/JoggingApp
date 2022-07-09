using JoggingApp.Core;
using Microsoft.EntityFrameworkCore;

namespace JoggingApp.EntityFramework
{
    public class UserStorage : IUserStorage
    {
        private readonly JoggingAppDbContext _context;
        public UserStorage(JoggingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task SaveAsync(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> FindByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Email == email && user.Password == password);
        }
    }
}
