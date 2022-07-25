namespace JoggingApp.Core.Users
{
    public class UserActivationToken
    {
        public UserActivationToken(DateTime validFrom, DateTime validTo, User user)
        {
            Id = Guid.NewGuid();
            ValidFrom = validFrom;
            ValidTo = validTo;
            User = user;
        }

        private UserActivationToken()
        {
        }

        public Guid Id { get; private set; }

        public DateTime ValidFrom { get; private set; }

        public DateTime ValidTo { get; private set; }

        public Guid UserId { get; private set; }

        public User User { get; private set; }
    }
}
