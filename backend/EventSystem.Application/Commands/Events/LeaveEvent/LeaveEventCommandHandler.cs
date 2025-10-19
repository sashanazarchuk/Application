using EventSystem.Application.Exceptions;
using EventSystem.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Events.LeaveEvent
{
    internal class LeaveEventCommandHandler : IRequestHandler<LeaveEventCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<LeaveEventCommandHandler> _logger;
        public LeaveEventCommandHandler(IEventRepository eventRepository, ILogger<LeaveEventCommandHandler> logger)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Handle(LeaveEventCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {UserId} is attempting to leave event {EventId}", request.UserId, request.EventId);

            var participant = await _eventRepository.GetParticipantAsync(request.EventId, request.UserId, cancellationToken);
            if (participant == null)
                throw new NotFoundException("Participant record not found.");

            await _eventRepository.LeaveEventAsync(participant, cancellationToken);
            _logger.LogInformation("User {UserId} successfully left event {EventId}", request.UserId, request.EventId);
        }
    }
}
