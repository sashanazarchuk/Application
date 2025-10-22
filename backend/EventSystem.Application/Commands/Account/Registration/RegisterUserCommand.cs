using EventSystem.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Account.Registration
{
    public record RegisterUserCommand(RegisterUserDto dto): IRequest<TokenDto>;    
}