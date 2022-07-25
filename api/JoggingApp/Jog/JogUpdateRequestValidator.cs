using FluentValidation;

namespace JoggingApp.Jogs
{
    public class JogUpdateRequestValidator : AbstractValidator<JogUpdateRequest>
    {
        public JogUpdateRequestValidator()
        {
            RuleFor(x => x.Distance).NotEmpty();
            RuleFor(x => x.Time).NotEmpty();
        }
    }
}
