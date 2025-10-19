using EventSystem.Application.Commands.Events.CreateEvent;
using EventSystem.Application.DTOs.Event;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Validators.Events
{
    public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
    {
        public CreateEventDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.")
                .Must(BeInTheFuture).WithMessage("Date cannot be in the past.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.");
        }

        private bool BeInTheFuture(DateTime date)
        {
            return date > DateTime.UtcNow;
        }
    }
}
