using EventSystem.Application.Exceptions;
using EventSystem.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Events.DeleteEvent
{
    internal class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<DeleteEventCommandHandler> _logger;
        public DeleteEventCommandHandler(IEventRepository eventRepository, ILogger<DeleteEventCommandHandler> logger)
        {
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting event with ID {EventId} by user {UserId}", request.EventId, request.UserId);

            var domainEvent = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
            if (domainEvent == null)
            {
                _logger.LogWarning("Event with ID {EventId} not found", request.EventId);
                throw new NotFoundException($"Event with ID {request.EventId} not found.");
            }

            if (domainEvent.AdminId != request.UserId)
                throw new ForbiddenException("Only the event organizer can delete this event.");
            
            await _eventRepository.DeleteAsync(domainEvent, cancellationToken);
            _logger.LogInformation("Event with ID {EventId} deleted successfully", request.EventId);

        }
    }
}