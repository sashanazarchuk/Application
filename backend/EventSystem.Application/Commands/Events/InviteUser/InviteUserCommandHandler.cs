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

namespace EventSystem.Application.Commands.Events.InviteUser
{
    internal class InviteUserCommandHandler : IRequestHandler<InviteUserCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<InviteUserCommandHandler> _logger;
        public InviteUserCommandHandler(IEventRepository eventRepository, ILogger<InviteUserCommandHandler> logger)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Handle(InviteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {AdminId} is attempting to invite user {InvitedUserId} to event {EventId}", request.AdminId, request.InvitedUserId, request.EventId);
            var eventEntity = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException("Event not found");

            if (eventEntity.AdminId != request.AdminId)
                throw new ForbiddenException("Only the event organizer can invite users to this event.");

            if (eventEntity.Type != EventType.Private)
                throw new ForbiddenException("Invitations are only for private events.");

            var existing = await _eventRepository.GetParticipantAsync(request.EventId, request.InvitedUserId, cancellationToken);
            if (existing != null)
                throw new BusinessException("User already invited or joined this event.");

            var participant = new Participant
            {
                EventId = request.EventId,
                UserId = request.InvitedUserId,
                JoinedAt = DateTime.UtcNow
            };

            await _eventRepository.JoinEventAsync(participant, cancellationToken);
            _logger.LogInformation("User {InvitedUserId} has been invited to event {EventId} by admin {AdminId}", request.InvitedUserId, request.EventId, request.AdminId);
        }
    }
}