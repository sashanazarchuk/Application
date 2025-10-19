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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FetchPublicEvents()
        {
            var events = await _mediator.Send(new GetAllPublicEventsQuery());
            return Ok(events);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCarById(Guid id)
        {
            var carBrand = await _mediator.Send(new GetEventByIdQuery(id));
            return Ok(carBrand);
        }
    }
}