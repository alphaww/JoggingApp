using FluentValidation;

namespace JoggingApp.Users
{
    public class UserAuthRequestValidator : AbstractValidator<UserAuthRequest>
    {
        public UserAuthRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
