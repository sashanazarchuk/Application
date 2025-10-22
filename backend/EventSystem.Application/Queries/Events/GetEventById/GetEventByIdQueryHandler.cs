using AutoMapper;
using EventSystem.Application.DTOs.Event;
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

namespace EventSystem.Application.Queries.Events.GetEventById
{
    internal class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEventByIdQueryHandler> _logger;
        private readonly ICurrentUserService _currentUserService;


        public GetEventByIdQueryHandler(IEventRepository eventRepository, IMapper mapper, ILogger<GetEventByIdQueryHandler> logger, ICurrentUserService currentUserService)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }
        public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching event with ID {EventId}", request.EventId);

            var domainEvent = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (domainEvent is null)
            {
                _logger.LogWarning("Event with ID {EventId} not found", request.EventId);
                throw new NotFoundException("Event was not found");
            }

            var dto = _mapper.Map<EventDto>(domainEvent);

            var currentUserId = _currentUserService.UserId;
            
            if (currentUserId.HasValue)
            {
                dto.IsJoined = domainEvent.Participants.Any(p => p.UserId == currentUserId.Value);
                dto.IsAdmin = domainEvent.AdminId == currentUserId.Value;
            }

            _logger.LogInformation("Event with ID {EventId} fetched successfully", request.EventId);
            return dto;
        }
    }
}