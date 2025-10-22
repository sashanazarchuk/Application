using AutoMapper;
using EventSystem.Application.DTOs.Event;
using EventSystem.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Queries.Events.GetUsersEvents
{
    internal class GetUsersEventsQueryHandler : IRequestHandler<GetUsersEventsQuery, IEnumerable<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUsersEventsQueryHandler> _logger;

        public GetUsersEventsQueryHandler(IEventRepository eventRepository, IMapper mapper, ILogger<GetUsersEventsQueryHandler> logger)
        {
            _eventRepository = eventRepository ?? throw new ArgumentException(nameof(eventRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        public async Task<IEnumerable<EventDto>> Handle(GetUsersEventsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching events for user {UserId}", request.UserId);
            var events = await _eventRepository.FetchUserEventsAsync(request.UserId, cancellationToken);

            _logger.LogInformation("Fetched {EventCount} events for user {UserId}", events.Count(), request.UserId);
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }
    }
}
