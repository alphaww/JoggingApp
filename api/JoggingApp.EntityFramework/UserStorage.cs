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

        public async Task InsertAsync(User user, CancellationToken cancellation)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task ConfirmAsync(User user, CancellationToken cancellation)
        {
            _context.ChangeTracker.Clear();
            _context.Users.Update(user);
            _context.UserActivationTokens.RemoveRange(user.ActivationTokens);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<User> FindByEmailAsync(string email, CancellationToken cancellation)
        {
            return await _context.Users
                .Include(user => user.ActivationTokens)
                .SingleOrDefaultAsync(user => user.Email == email, cancellation);
        }

        public async Task<User> FindActiveByEmailAndPasswordAsync(string email, string password, CancellationToken cancellation)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Email == email && user.Password == password 
                    && user.State == UserState.Active, cancellation);
        }

        public async Task<User> FindByActivationTokenId(Guid activationTokenId, CancellationToken cancellation)
        {
            return await _context.Users
                .Include(user => user.ActivationTokens)
                .SingleOrDefaultAsync(user => user.ActivationTokens.Any(t => t.Id == activationTokenId));
        }
    }
}
