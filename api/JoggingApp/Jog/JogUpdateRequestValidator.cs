using FluentValidation;

namespace JoggingApp.Jogs
{
    public class JogUpdateRequestValidator : AbstractValidator<JogUpdateRequest>
    {
        public JogUpdateRequestValidator()
        {
            RuleFor(x => x.Distance)
                .NotEmpty()
                .Must(x => x > 0 && x <= 150000)
                .WithMessage("Distance must be integer in range [1 - 150000] meters");

            RuleFor(x => x.Time)
                .NotEmpty()
                .Must(x => x is not null 
                        && x.Hours >= 0 && x.Hours < 24
                        && x.Minutes >= 0 && x.Minutes < 60
                        && x.Seconds >= 0 && x.Seconds < 60)
                .WithMessage("You must specify your running time. Running time between 00:00:00 and 23:59:59 in hours:minutes:seconds allowed.");
        }
    }
}
