using System.Text.Json.Serialization;
using JoggingApp.Core.Clock;
using JoggingApp.Core.Crypto;
using JoggingApp.Core.Jog.DomainEvents;

namespace JoggingApp.Core.Users
{
    public class User : Entity
    {
        public static User Create(string email, string password, IHashService hashService, IClock clock)
        {
            var user = new User(Guid.NewGuid(), email, hashService.Hash(password), UserState.Inactive);
            //Expiration time shouldd not be hardcoded. But will leave it as is for this demo purpose
            var activationToken = new UserActivationToken(clock.Now, clock.Now.AddMinutes(2), user);
            user.ActivationTokens.Add(activationToken);
            user.RaiseUserRegisteredDomainEvent();
            return user;
        }

        private User(Guid id, string email, string password, UserState state) : base(id)
        {
            Email = email;
            Password = password;
            State = state;
            ActivationTokens = new HashSet<UserActivationToken>();
        }

        public string Email { get; }

        public string Password { get; }

        public UserState State { get; private set; }

        public ICollection<UserActivationToken> ActivationTokens { get; }

        public void RaiseUserRegisteredDomainEvent()
        {
            RaiseDomainEvent(new UserRegisteredDomainEvent(this));
        }

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
