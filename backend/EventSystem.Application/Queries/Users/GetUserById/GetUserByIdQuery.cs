using EventSystem.Application.DTOs.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Queries.Users.GetUserById
{
    public record GetUserByIdQuery(Guid UserId):IRequest<UserDto>;
}
