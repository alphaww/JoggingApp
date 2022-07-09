namespace JoggingApp.Core
{
    public interface IUserStorage
    {
        public Task SaveAsync(User user);
        public Task<User> FindByEmailAndPasswordAsync(string email, string password);
    }
}
