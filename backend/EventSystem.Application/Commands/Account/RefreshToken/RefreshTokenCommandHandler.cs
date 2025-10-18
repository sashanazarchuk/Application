using EventSystem.Application.DTOs.Auth;
using EventSystem.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Account.RefreshToken
{
    internal class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenDto>
    {
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenCommandHandler(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        }

        public async Task<TokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (request?.refreshToken == null)
                throw new ArgumentNullException(nameof(request.refreshToken));

            return await _jwtTokenService.RefreshToken(request.refreshToken);
        }
    }
}