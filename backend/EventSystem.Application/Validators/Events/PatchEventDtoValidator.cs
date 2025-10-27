using EventSystem.Application.DTOs.Event;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Validators.Events
{
    public class PatchEventDtoValidator : AbstractValidator<PatchEventDto>
    {
        public PatchEventDtoValidator()
        {
            RuleFor(x => x.Title)
               .NotEmpty()
               .When(x => x.Title != null)
               .WithMessage("Title cannot be empty if provided.");

            RuleFor(x => x.Location)
               .NotEmpty()
               .When(x => x.Location != null)
               .WithMessage("Location cannot be empty if provided.");

            RuleFor(x => x.Date)
               .Must(BeInTheFuture)
               .When(x => x.Date.HasValue)
               .WithMessage("Date cannot be in the past.");

            RuleFor(x => x.TagNames)
                .NotEmpty()
                .Must(tags => tags == null || tags.Any())
                .WithMessage("At least one tag is required while updating.");
        }

        private bool BeInTheFuture(DateTime? date)
        {
            return date!.Value > DateTime.UtcNow;
        }
    }
}