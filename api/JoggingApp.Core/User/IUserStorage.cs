namespace JoggingApp.Core.Users
{
    public interface IUserStorage
    {
        public Task SaveAsync(User user, CancellationToken cancellation);
        public Task<User> FindByEmailAsync(string email, CancellationToken cancellation);
        public Task<User> FindByEmailAndPasswordAsync(string email, string password, CancellationToken cancellation);
    }
}
