using AutoMapper;
using EventSystem.Application.DTOs.Event;
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

namespace EventSystem.Application.Queries.Events.GetEventsByTag
{
    internal class GetEventsByTagQueryHandler : IRequestHandler<GetEventsByTagQuery, IEnumerable<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<GetEventsByTagQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public GetEventsByTagQueryHandler(IEventRepository eventRepository, ILogger<GetEventsByTagQueryHandler> logger, IMapper mapper, ICurrentUserService currentUserService)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<IEnumerable<EventDto>> Handle(GetEventsByTagQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving events with tags: {Tags}", request.Tags == null ? "None" : string.Join(", ", request.Tags));

            var events = (request.Tags == null || !request.Tags.Any())
            ? await _eventRepository.GetAllAsync(cancellationToken)
            : await _eventRepository.GetEventsByTagsAsync(request.Tags, cancellationToken);

            var currentUserId = _currentUserService.UserId;

            var eventDtos = events.Select(domainEvent =>
            {
                var dto = _mapper.Map<EventDto>(domainEvent);

                if (currentUserId.HasValue)
                {
                    dto.IsJoined = domainEvent.Participants.Any(p => p.UserId == currentUserId.Value);
                    dto.IsAdmin = domainEvent.AdminId == currentUserId.Value;
                }

                return dto;
            });


            _logger.LogInformation("Returning {EventCount} event DTOs", eventDtos.Count());
            return eventDtos;
        }
    }
}
