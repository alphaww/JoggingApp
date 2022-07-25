using JoggingApp.Core.Crypto;

namespace JoggingApp.Core.Users
{
    public class User
    {
        public static (User, UserActivationToken) Create(string email, string password, int expiresInMinutes, IHashService hashService)
        {
            var user = new User(Guid.NewGuid(), email, hashService.Hash(password), UserState.Inactive);
            var activationToken = new UserActivationToken(DateTime.UtcNow, DateTime.UtcNow.AddMinutes(expiresInMinutes), user);
            user.ActivationTokens.Add(activationToken);
            return (user, activationToken);
        }

        private User(Guid id, string email, string password, UserState state)
        {
            Id = id;
            Email = email;
            Password = password;
            State = state;
            ActivationTokens = new HashSet<UserActivationToken>();
        }

        public Guid Id { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public UserState State { get; private set; }

        public ICollection<UserActivationToken> ActivationTokens { get; private set; }

        public void Activate()
        {
            State = UserState.Active;
            ActivationTokens.Clear();
        }

        public bool HasValidActivationToken()
        {
            return ActivationTokens.Any(x => x.ValidFrom <= DateTime.UtcNow && x.ValidTo >= DateTime.UtcNow);
        }

    }
}
