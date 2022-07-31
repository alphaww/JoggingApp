using FluentValidation;

namespace JoggingApp.Jogs
{
    public class JogUpdateRequestValidator : AbstractValidator<JogUpdateRequest>
    {
        public JogUpdateRequestValidator()
        {
            RuleFor(x => x.Distance)
                .NotEmpty()
                .Must(x => x > 0 && x <= 500000)
                .WithMessage("Distance must be integer in range between 1 and 500000 meters");

            RuleFor(x => x.Time)
                .NotEmpty()
                .Must(x => x.ToTimeSpan().TotalSeconds < 24 * 60 * 60
                        && x.ToTimeSpan().TotalSeconds > 0)
                .WithMessage("You must specify your running time. Running time between 00:00:01 and 23:59:59 in hours:minutes:seconds allowed.");
        }
    }
}
