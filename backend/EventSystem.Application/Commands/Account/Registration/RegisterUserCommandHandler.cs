using AutoMapper;
using EventSystem.Application.DTOs.Auth;
using EventSystem.Application.Interfaces.Repositories;
using EventSystem.Application.Interfaces.Services;
using EventSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Account.Registration
{
    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, TokenDto>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IUserRepository userRepository, ILogger<RegisterUserCommandHandler> logger, IUserService userService, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<TokenDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering user {Email}", request.dto.Email);

            var domainUser = _mapper.Map<User>(request.dto);

            var identityId = await _userService.RegisterAsync(domainUser, request.dto.Email, request.dto.Password);

            domainUser.Id = identityId;

            await _userRepository.AddAsync(domainUser);

            _logger.LogInformation("User {Email} registered successfully", request.dto.Email);
            return await _userService.GenerateTokensAsync(domainUser.Id, request.dto.Email);
        }
    }
}