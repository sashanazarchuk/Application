using AutoMapper;
using EventSystem.Application.DTOs.Event;
using EventSystem.Application.DTOs.Tag;
using EventSystem.Application.DTOs.Users;
using EventSystem.Application.Exceptions;
using EventSystem.Application.Interfaces.Repositories;
using EventSystem.Application.Interfaces.Services;
using EventSystem.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Services
{
    public class UserSnapshotService : IUserSnapshotService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserSnapshotService> _logger;

        public UserSnapshotService(IUserRepository userRepository, IEventRepository eventRepository, ITagRepository tagRepository, ICurrentUserService currentUserService, IMapper mapper, ILogger<UserSnapshotService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        public async Task<UserSnapshotDto> GetUserSnapshotAsync( CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to fetch user snapshot");

            var currentUserId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException();

            var user = await _userRepository.GetByIdAsync(currentUserId, cancellationToken)
                ?? throw new NotFoundException("User not found");

            _logger.LogInformation("Fetching events and tags for user ID: {UserId}", currentUserId);

            var userEvents = await _eventRepository.FetchUserEventsAsync(currentUserId, cancellationToken);
            var allTags = await _tagRepository.GetAllAsync(cancellationToken);
            var publicEvents = await _eventRepository.GetAllAsync(cancellationToken);

            _logger.LogInformation("Mapping data to UserSnapshotDto for user ID: {UserId}", currentUserId);

            return new UserSnapshotDto
            {
                User = _mapper.Map<UserDto>(user),
                AdminEvents = MapEvents(userEvents.Where(e => e.AdminId == currentUserId), currentUserId),
                JoinedEvents = MapEvents(userEvents.Where(e => e.AdminId != currentUserId), currentUserId),
                PublicEvents = MapEvents(publicEvents, currentUserId),
                AllTags = _mapper.Map<List<TagDto>>(allTags)
            };
        }

        private List<EventDto> MapEvents(IEnumerable<Event> events, Guid currentUserId)
        {
            return events.Select(domainEvent =>
            {
                var dto = _mapper.Map<EventDto>(domainEvent);
                dto.IsAdmin = domainEvent.AdminId == currentUserId;
                dto.IsJoined = domainEvent.Participants.Any(p => p.UserId == currentUserId);
                return dto;
            }).ToList();
        }
    }
}
