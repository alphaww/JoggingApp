using JoggingApp.Core.Crypto;

namespace JoggingApp.Core.Users
{
    public class User
    {
        public static User Create(string email, string password, IHashService hashService)
        {
            return new User(Guid.NewGuid(), email, hashService.Hash(password));
        }

        public User(Guid id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public Guid Id { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

    }
}
