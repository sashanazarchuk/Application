using AutoMapper;
using EventSystem.Application.DTOs.Event;
using EventSystem.Application.Interfaces.Repositories;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllPublicEventsQueryHandler(IEventRepository eventRepository, ILogger<GetAllPublicEventsQueryHandler> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public async Task<IEnumerable<EventDto>> Handle(GetAllPublicEventsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all public events");
            var events = await _eventRepository.GetAllAsync(cancellationToken);


            Guid? currentUserId = null;
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var idStr = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (Guid.TryParse(idStr, out var id))
                    currentUserId = id;
            }

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

            return eventsDto;
        }
    }
}