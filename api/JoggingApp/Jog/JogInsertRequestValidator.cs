using FluentValidation;
using System;
using System.Text.RegularExpressions;

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

            //RuleFor(x => x.Time)
            //    .NotEmpty()
            //    .Must(x => Regex.IsMatch(x, "^(0?[0-9]|1[0-9]|2[0-3]):[0-5]?[0-9]:[0-5]?[0-9]$"))
            //    .WithMessage("Time must be a valid hh:mm:ss or h:m:s time span string within the 24 hour range");

            RuleFor(x => x.Time)
                .NotEmpty()
                .Must(x => x.Hours >= 0 && x.Hours < 24
                        && x.Minutes >= 0 && x.Minutes < 60
                        && x.Seconds >= 0 && x.Seconds < 60)
                .WithMessage("You must specify your running time. Running time must be in range [00:00:00 - 24:00:00] in {hours:minutes:seconds}.");
        }
    }
}
