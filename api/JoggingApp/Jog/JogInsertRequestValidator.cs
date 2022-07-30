using FluentValidation;

namespace JoggingApp.Jogs
{
    public class JogInsertRequestValidator : AbstractValidator<JogInsertRequest>
    {
        public JogInsertRequestValidator()
        {
            RuleFor(x => x.Distance)
                .NotEmpty()
                .Must(x => x > 0 && x <= 500000)
                .WithMessage("Distance must be integer in range [1 - 500000] meters");

            RuleFor(x => x.Time)
                .NotEmpty()
                .Must(x => x.Hours >= 0 && x.Hours < 24
                        && x.Minutes >= 0 && x.Minutes < 60
                        && x.Seconds >= 0 && x.Seconds < 60)
                .WithMessage("You must specify your running time. Running time must be in range [00:00:00 - 24:00:00] in {hours:minutes:seconds}.");

            RuleFor(x => x.Coordinates)
                .Must(x => x == null || (x.Latitude >= -90 && x.Latitude <= 90))
                .WithMessage("Latitude must be between -90 and 90 degrees inclusive.");

            RuleFor(x => x.Coordinates)
               .Must(x => x == null || (x.Longitude >= -180 && x.Longitude <= 180))
               .WithMessage("Longitude must be between -180 and 180 degrees inclusive.");
        }
    }
}
