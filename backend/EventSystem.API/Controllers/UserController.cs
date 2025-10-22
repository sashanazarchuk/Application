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

        /// <summary>
        /// Get events of the authenticated user
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves all events created or joined by the authenticated user.
        /// Requires the user to be authorized. Returns a list of event details.
        /// </remarks>
        [HttpGet("me/events")]
        public async Task<IActionResult> GetMyEvents()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var userId = Guid.Parse(userIdStr);
            var events = await _mediator.Send(new GetUsersEventsQuery(userId));
            return Ok(events);
        }

        /// <summary>
        /// Get the authenticated user's details
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves the profile information of the currently authenticated user.
        /// Requires the user to be authorized. Returns user details if found.
        /// </remarks>
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