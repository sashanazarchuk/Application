using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Events.DeleteEvent
{
    public record DeleteEventCommand(Guid EventId, Guid UserId) :IRequest;    
}