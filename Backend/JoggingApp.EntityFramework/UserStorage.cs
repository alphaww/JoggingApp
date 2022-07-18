using JoggingApp.Core;
using JoggingApp.Core.Users;
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

        public async Task SaveAsync(User user, CancellationToken cancellation)
        {
            _context.Add(user);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<User> FindByEmailAsync(string email, CancellationToken cancellation)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Email == email, cancellation);
        }

        public async Task<User> FindByEmailAndPasswordAsync(string email, string password, CancellationToken cancellation)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Email == email && user.Password == password, cancellation);
        }
    }
}
