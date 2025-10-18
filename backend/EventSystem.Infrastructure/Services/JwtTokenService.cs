using AutoMapper;
using EventSystem.Application.DTOs.Auth;
using EventSystem.Application.Exceptions;
using EventSystem.Application.Interfaces.Services;
using EventSystem.Application.Settings;
using EventSystem.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Services
{
    internal class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public JwtTokenService(IOptions<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _mapper = mapper;
        }
        public string CreateAccessToken(ApplicationUserDto user)
        {
            var claims = new List<Claim>
            {
             new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
             new Claim(JwtRegisteredClaimNames.Email, user.Email),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(

                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                signingCredentials: signinCredentials,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<TokenDto> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new SecurityTokenException("Refresh token is required.");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new SecurityTokenException("Invalid or expired refresh token.");

            var applicationUserDto = _mapper.Map<ApplicationUserDto>(user);
            var newAccessToken = CreateAccessToken(applicationUserDto);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new BusinessException("Failed to update user refresh token.");

            return new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}