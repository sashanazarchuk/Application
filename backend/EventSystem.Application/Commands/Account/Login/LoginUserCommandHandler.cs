using AutoMapper;
using EventSystem.Application.DTOs.Auth;
using EventSystem.Application.Interfaces.Services;
using EventSystem.Application.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Account.Login
{
    internal class LoginCommandHandler : IRequestHandler<LoginUserCommand, TokenDto>
    {
        private readonly IUserService _userService;
        public LoginCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<TokenDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var userDto = await _userService.LoginAsync(request.dto.Email, request.dto.Password);
            return await _userService.GenerateTokensAsync(userDto.Id, userDto.Email);
        }
    }
}