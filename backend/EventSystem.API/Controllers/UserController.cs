using EventSystem.Application.Queries.Events.GetUsersEvents;
using EventSystem.Application.Queries.Users.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventSystem.API.Controllers
{
    [Route("users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("me/events")]
        public async Task<IActionResult> GetMyEvents()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var userId = Guid.Parse(userIdStr);
            var events = await _mediator.Send(new GetUsersEventsQuery(userId));
            return Ok(events);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetUserById()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var userId = Guid.Parse(userIdStr);
            var user = await _mediator.Send(new GetUserByIdQuery(userId));
            return Ok(user);
        }
    }
}
