using JoggingApp.Core.Clock;
using JoggingApp.Core.Crypto;

namespace JoggingApp.Core.Users
{
    public class User
    {
        public static (User, UserActivationToken) Create(string email, string password, IHashService hashService, IClock clock)
        {
            var user = new User(Guid.NewGuid(), email, hashService.Hash(password), UserState.Inactive);
            //Expiration time shouldd not be hardcoded. But will leave it as is for this demo purpose
            var activationToken = new UserActivationToken(clock.Now, clock.Now.AddMinutes(2), user);
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
        }

        public bool HasValidActivationToken(IClock clock)
        {
            return ActivationTokens.Any(x => x.ValidFrom <= clock.Now && x.ValidTo >= clock.Now);
        }

    }
}
