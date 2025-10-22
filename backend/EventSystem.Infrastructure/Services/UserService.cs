using AutoMapper;
using EventSystem.Application.DTOs.Auth;
using EventSystem.Application.Exceptions;
using EventSystem.Application.Interfaces.Services;
using EventSystem.Application.Settings;
using EventSystem.Domain.Entities;
using EventSystem.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Services
{
    internal class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;
        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, IJwtTokenService jwtTokenService, IOptions<JwtSettings> jwtSettings)
        {
            _jwtTokenService = jwtTokenService;
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Guid> RegisterAsync(User domainUser, string email, string password)
        {
            var appUser = _mapper.Map<ApplicationUser>(domainUser);
            appUser.UserName = email;
            appUser.Email = email;

            var result = await _userManager.CreateAsync(appUser, password);

            if (!result.Succeeded && result.Errors.Any(e => e.Code == "DuplicateEmail"))
                throw new BusinessException("Email already exists.");

            return appUser.Id;
        }

        public async Task<ApplicationUserDto> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new BusinessException("Invalid email or password.");

            var isValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isValid)
                throw new BusinessException("Invalid email or password.");

            return _mapper.Map<ApplicationUserDto>(user);
        }

        public async Task LogoutAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException("User not found");

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userManager.UpdateAsync(user);
        }


        public async Task<TokenDto> GenerateTokensAsync(Guid userId, string email)
        {
            var userDto = new ApplicationUserDto
            {
                Id = userId,
                Email = email
            };

            var accessToken = _jwtTokenService.CreateAccessToken(userDto);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

            await SetRefreshTokenAsync(userId, refreshToken, refreshTokenExpiry);

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        private async Task SetRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiryTime)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException("User not found");

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiryTime;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BusinessException("Failed to update refresh token");
        }
    }
}