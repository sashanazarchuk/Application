using EventSystem.Application.Commands.Account.Login;
using EventSystem.Application.Commands.Account.Logout;
using EventSystem.Application.Commands.Account.RefreshToken;
using EventSystem.Application.Commands.Account.Registration;
using EventSystem.Application.DTOs.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventSystem.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <remarks>
        /// This endpoint creates a new user with the provided information.
        /// Ensure that the email is unique.
        /// </remarks>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var command = new RegisterUserCommand(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <remarks>
        /// This endpoint authenticates a user with their email and password.
        /// Returns JWT access token and refresh token.
        /// </remarks>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var command = new LoginUserCommand(dto);
            var token = await _mediator.Send(command);
            return Ok(token);
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        /// <remarks>
        /// This endpoint logs out the authenticated user by invalidating their refresh token
        /// and ending their session. Requires the user to be authorized.
        /// </remarks>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);

            var command = new LogoutUserCommand(userId);
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Refresh access token
        /// </summary>
        /// <remarks>
        /// This endpoint generates new authentication tokens using a valid refresh token.
        /// </remarks>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
        {
            var command = new RefreshTokenCommand(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}