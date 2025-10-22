using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Events.InviteUser
{
    public record InviteUserCommand(Guid EventId, Guid AdminId, Guid InvitedUserId) : IRequest;
}