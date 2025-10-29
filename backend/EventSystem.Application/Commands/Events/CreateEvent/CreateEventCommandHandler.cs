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

namespace EventSystem.Application.Commands.Events.CreateEvent
{
    internal class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEventCommandHandler> _logger;
        private readonly ITagService _tagService;
        public CreateEventCommandHandler(IEventRepository eventRepository, IMapper mapper, ILogger<CreateEventCommandHandler> logger, ITagService tagService)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        }
        public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating event {Title} by admin {AdminId}", request.dto.Title, request.AdminId);

            var domainEvent = _mapper.Map<Event>(request.dto);
            domainEvent.AdminId = request.AdminId;

            var tags = await _tagService.GetOrCreateTagsAsync(request.dto.TagNames, cancellationToken);

            // Прив’язка до події
            domainEvent.EventTags = tags
                .Select(t => new EventTag { TagId = t.Id })
                .ToList();

            var createdEvent = await _eventRepository.AddAsync(domainEvent, cancellationToken);

            _logger.LogInformation("Event {Title} created successfully with ID {EventId}", createdEvent.Title, createdEvent.Id);

            return _mapper.Map<EventDto>(createdEvent);
        }
    }
}