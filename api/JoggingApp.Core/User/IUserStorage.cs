namespace JoggingApp.Core.Users
{
    public interface IUserStorage
    {
        public Task InsertAsync(User user, CancellationToken cancellation);
        public Task ConfirmAsync(User user, CancellationToken cancellation);
        public Task<User> FindByEmailAsync(string email, CancellationToken cancellation);
        public Task<User> FindActiveByEmailAndPasswordAsync(string email, string password, CancellationToken cancellation);
        public Task<User> FindByActivationTokenId(Guid activationTokenId, CancellationToken cancellation);
    }
}
