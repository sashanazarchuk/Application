using AutoMapper;
using EventSystem.Application.DTOs.Event;
using EventSystem.Application.DTOs.Users;
using EventSystem.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Queries.Users.GetUserById
{
    internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetUserByIdQueryHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", request.UserId);
            var domainUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            _logger.LogInformation("User with ID {UserId} fetched successfully", request.UserId);
            return _mapper.Map<UserDto>(domainUser);

        }
    }
}
