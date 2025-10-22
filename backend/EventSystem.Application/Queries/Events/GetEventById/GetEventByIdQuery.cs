using EventSystem.Application.DTOs.Event;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Queries.Events.GetEventById
{
    public record GetEventByIdQuery(Guid EventId):IRequest<EventDto>;


}
