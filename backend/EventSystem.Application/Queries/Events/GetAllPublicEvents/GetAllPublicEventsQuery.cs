using EventSystem.Application.DTOs.Event;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Queries.Events.GetAllPublicEvents
{
    public record GetAllPublicEventsQuery:IRequest<IEnumerable<EventDto>>;


}
