using EventSystem.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string CreateAccessToken(ApplicationUserDto user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}