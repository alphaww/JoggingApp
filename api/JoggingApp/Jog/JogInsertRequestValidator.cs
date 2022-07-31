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
                .WithMessage("Distance must be integer in range between 1 and 500000 meters");

            RuleFor(x => x.Time)
                .NotEmpty()
                .Must(x => x.ToTimeSpan().TotalSeconds < 24 * 60 * 60 
                        && x.ToTimeSpan().TotalSeconds > 0)
                .WithMessage("You must specify your running time. Running time between 00:00:01 and 23:59:59 in hours:minutes:seconds allowed.");

            RuleFor(x => x.Coordinates)
                .Must(x => x == null || (x.Latitude >= -90 && x.Latitude <= 90))
                .WithMessage("Latitude must be between -90 and 90 degrees inclusive.");

            RuleFor(x => x.Coordinates)
               .Must(x => x == null || (x.Longitude >= -180 && x.Longitude <= 180))
               .WithMessage("Longitude must be between -180 and 180 degrees inclusive.");
        }
    }
}
