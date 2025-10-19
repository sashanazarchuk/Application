using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Events.LeaveEvent
{
    public record LeaveEventCommand(Guid EventId, Guid UserId) : IRequest;

}
