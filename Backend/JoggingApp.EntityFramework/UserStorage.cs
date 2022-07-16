using JoggingApp.Core;
using Microsoft.Data.SqlClient;
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

        public async Task<UserRegistrationResult> SaveAsync(User user)
        {
            _context.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            //Catch unique constraint violation by inspecting 2601 error code returned by SQL server
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException as SqlException;
                if (innerException != null && innerException.Number == 2601)
                {
                    return UserRegistrationResult.Duplicate;
                }
                else
                {
                    throw;
                }
            }
            return UserRegistrationResult.Success;
        }

        public async Task<User> FindByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Email == email && user.Password == password);
        }
    }
}
