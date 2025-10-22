using EventSystem.Application.DTOs.Event;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Events.CreateEvent
{
    public record CreateEventCommand(CreateEventDto dto, Guid AdminId):IRequest<EventDto>;
}