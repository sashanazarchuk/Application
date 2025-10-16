using EventSystem.Application.DTOs.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Account.RefreshToken
{
    public record RefreshTokenCommand(TokenDto Token) : IRequest<TokenDto>;
}
