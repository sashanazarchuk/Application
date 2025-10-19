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

namespace EventSystem.Application.Queries.Events.GetAllPublicEvents
{
    internal class GetAllPublicEventsQueryHandler : IRequestHandler<GetAllPublicEventsQuery, IEnumerable<EventDto>>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<GetAllPublicEventsQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetAllPublicEventsQueryHandler(IEventRepository eventRepository, ILogger<GetAllPublicEventsQueryHandler> logger, IMapper mapper)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<IEnumerable<EventDto>> Handle(GetAllPublicEventsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving all public events");
            var events = await _eventRepository.GetAllAsync(cancellationToken);

            _logger.LogInformation("Retrieved {EventCount} public events", events.Count());
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }
    }
}