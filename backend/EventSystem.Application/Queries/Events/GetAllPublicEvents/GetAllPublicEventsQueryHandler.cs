using AutoMapper;
using EventSystem.Application.DTOs.Event;
using EventSystem.Application.Interfaces.Repositories;
using EventSystem.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Queries.Events.GetAllPublicEvents
{
    internal class GetAllPublicEventsQueryHandler : IRequestHandler<GetAllPublicEventsQuery, IEnumerable<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<GetAllPublicEventsQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetAllPublicEventsQueryHandler(IEventRepository eventRepository, ILogger<GetAllPublicEventsQueryHandler> logger, IMapper mapper, ICurrentUserService currentUserService)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }
        public async Task<IEnumerable<EventDto>> Handle(GetAllPublicEventsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all public events");
            var events = await _eventRepository.GetAllAsync(cancellationToken);

            var currentUserId = _currentUserService.UserId;

            var eventsDto = events.Select(e =>
            {
                var dto = _mapper.Map<EventDto>(e);

                if (currentUserId.HasValue)
                {
                    dto.IsJoined = e.Participants.Any(p => p.UserId == currentUserId.Value);
                    dto.IsAdmin = e.AdminId == currentUserId.Value;
                }

                return dto;
            });

            _logger.LogInformation("Returning {EventCount} event DTOs", eventsDto.Count());
            return eventsDto;
        }
    }
}