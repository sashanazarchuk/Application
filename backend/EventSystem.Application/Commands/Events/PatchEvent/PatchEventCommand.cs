using EventSystem.Application.DTOs.Event;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Events.PatchEvent
{
    public record PatchEventCommand(Guid EventId, Guid UserId, JsonPatchDocument<PatchEventDto> PatchDoc):IRequest<EventDto>;
}