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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }

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

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var result = await _mediator.Send(new RefreshTokenCommand(tokenDto));
            return Ok(result);
        }
    }
}