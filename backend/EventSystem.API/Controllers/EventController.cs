using EventSystem.Application.Commands.Events.CreateEvent;
using EventSystem.Application.Commands.Events.DeleteEvent;
using EventSystem.Application.Commands.Events.InviteUser;
using EventSystem.Application.Commands.Events.JoinEvent;
using EventSystem.Application.Commands.Events.LeaveEvent;
using EventSystem.Application.Commands.Events.PatchEvent;
using EventSystem.Application.DTOs.Event;
using EventSystem.Application.Queries.Events.GetAllPublicEvents;
using EventSystem.Application.Queries.Events.GetEventById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventSystem.API.Controllers
{
    [Route("events")]
    [ApiController]
    [Authorize]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EventController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Create a new event
        /// </summary>
        /// <remarks>
        /// This endpoint allows an authenticated user to create a new event with the provided details.
        /// The request must include all required information in the <c>CreateEventDto</c>.
        /// Returns the created event's details upon success.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var command = new CreateEventCommand(dto, Guid.Parse(userId));
            var createdEvent = await _mediator.Send(command);
            return Ok(createdEvent);
        }

        /// <summary>
        /// Partially update an event
        /// </summary>
        /// <remarks>
        /// This endpoint allows the authenticated user to apply partial updates to an event
        /// using a JSON Patch document. Only the fields included in the patch will be updated.
        /// </remarks>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEvent([FromBody] JsonPatchDocument<PatchEventDto> patchDoc, Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();

            var userId = Guid.Parse(userIdString);
            var command = new PatchEventCommand(id, userId, patchDoc);
            var updatedEvent = await _mediator.Send(command);
            return Ok(updatedEvent);
        }

        /// <summary>
        /// Delete an event
        /// </summary>
        /// <remarks>
        /// This endpoint allows the authenticated user to delete an event they own.
        /// Returns 204 No Content on successful deletion.
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();

            var userId = Guid.Parse(userIdString);
            await _mediator.Send(new DeleteEventCommand(id, userId));
            return NoContent();
        }

        /// <summary>
        /// Join an event
        /// </summary>
        /// <remarks>
        /// This endpoint allows the authenticated user to join an public event.
        /// Returns 204 No Content on success.
        /// </remarks>
        [HttpPost("{id}/join")]
        public async Task<IActionResult> JoinEvent(Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();

            var userId = Guid.Parse(userIdString);
            await _mediator.Send(new JoinEventCommand(id, userId));
            return NoContent();
        }

        /// <summary>
        /// Leave an event
        /// </summary>
        /// <remarks>
        /// This endpoint allows the authenticated user to leave an event they previously joined.
        /// Returns 204 No Content on success.
        /// </remarks>
        [HttpPost("{id}/leave")]
        public async Task<IActionResult> LeaveEvent(Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();

            var userId = Guid.Parse(userIdString);
            await _mediator.Send(new LeaveEventCommand(id, userId));
            return NoContent();
        }

        /// <summary>
        /// Invite a user to an event
        /// </summary>
        /// <remarks>
        /// This endpoint allows an authenticated event admin to invite another user to a specific event.
        /// - <c>id</c> in the route is the ID of the event.
        /// - <c>UserId</c> in the request body is the ID of the user being invited.
        /// Returns 204 No Content on success.
        /// </remarks>
        [HttpPost("{id}/invite")]
        public async Task<IActionResult> InviteUser(Guid id, [FromBody] InviteUserDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();

            var adminId = Guid.Parse(userIdString);

            var command = new InviteUserCommand(id, adminId, dto.UserId);
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Fetch all public events
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves a list of all public events. No authentication is required.
        /// Returns an array of event details.
        /// </remarks>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FetchPublicEvents()
        {
            var events = await _mediator.Send(new GetAllPublicEventsQuery());
            return Ok(events);
        }

        /// <summary>
        /// Get details of a specific event
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves detailed information about a specific event by its ID.
        /// No authentication is required. Returns the event details if found.
        /// </remarks>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            var events = await _mediator.Send(new GetEventByIdQuery(id));
            return Ok(events);
        }
    }
}