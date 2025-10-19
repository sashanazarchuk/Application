using EventSystem.Application.Exceptions;
using EventSystem.Application.Interfaces.Repositories;
using EventSystem.Domain.Entities;
using EventSystem.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Events.JoinEvent
{
    internal class JoinEventCommandHandler : IRequestHandler<JoinEventCommand>
    {
        private readonly IEventRepository _eventRepository;
        private ILogger<JoinEventCommandHandler> _logger;
        public JoinEventCommandHandler(IEventRepository eventRepository,  ILogger<JoinEventCommandHandler> logger)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Handle(JoinEventCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {UserId} is attempting to join event {EventId}", request.UserId, request.EventId);

            var eventEntity = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);

            if (eventEntity == null)
                throw new NotFoundException($"Event with ID {request.EventId} not found.");

            if (eventEntity.AdminId == request.UserId)
                throw new ForbiddenException("Organizer cannot join their own event.");

            if (eventEntity.Type != EventType.Public)
                throw new ForbiddenException("Cannot join a private event.");

            var existingParticipant = await _eventRepository.GetParticipantAsync(request.EventId, request.UserId, cancellationToken);

            if (existingParticipant != null)
                throw new ForbiddenException("User already joined this event.");

            var participant = new Participant
            {
                EventId = request.EventId,
                UserId = request.UserId,
                JoinedAt = DateTime.UtcNow
            };

            await _eventRepository.JoinEventAsync(participant, cancellationToken);
            _logger.LogInformation("User {UserId} successfully joined event {EventId}", request.UserId, request.EventId);
        }
    }
}