using AutoMapper;
using EventSystem.Application.DTOs.Event;
using EventSystem.Application.Exceptions;
using EventSystem.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Events.PatchEvent
{
    internal class PatchEventCommandHandler : IRequestHandler<PatchEventCommand, EventDto>
    {
        private readonly ILogger<PatchEventCommandHandler> _logger;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        public PatchEventCommandHandler(ILogger<PatchEventCommandHandler> logger, IEventRepository eventRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<EventDto> Handle(PatchEventCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Patching event {EventId} by user {UserId}", request.EventId, request.UserId);

            var existingEvent = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (existingEvent == null)
                throw new NotFoundException("Event not found.");

            if (existingEvent.AdminId != request.UserId)
                throw new ForbiddenException("Only the event organizer can edit this event.");

            var patchDto = new PatchEventDto();
            request.PatchDoc.ApplyTo(patchDto);

            _mapper.Map(patchDto, existingEvent);

            await _eventRepository.UpdateAsync(existingEvent, cancellationToken);

            _logger.LogInformation("Event {EventId} patched successfully", existingEvent.Id);
            return _mapper.Map<EventDto>(existingEvent);
        }
    }
}