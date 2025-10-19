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

namespace EventSystem.Application.Queries.Events.GetEventById
{
    internal class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEventByIdQueryHandler> _logger;
        public GetEventByIdQueryHandler(IEventRepository eventRepository, IMapper mapper, ILogger<GetEventByIdQueryHandler> logger)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }
        public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching event with ID {EventId}", request.EventId);
            var domainEvent = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);

            _logger.LogInformation("Event with ID {EventId} fetched successfully", request.EventId);
            return _mapper.Map<EventDto>(domainEvent);
        }
    }
}