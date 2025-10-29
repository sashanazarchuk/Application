using EventSystem.Application.DTOs.Users;
using EventSystem.Application.Queries.AI.GetAIResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventSystem.API.Controllers
{
    [Route("ai")]
    [ApiController]
    [Authorize]
    public class AIController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AIController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Sends a user's question to the AI Assistant and returns the generated response.
        /// </summary>
        /// <remarks>
        /// This endpoint is used to interact with the AI Assistant using natural-language.  
        /// The assistant analyzes the user's question and provides a response based on event data.  
        /// </remarks>
        [HttpPost("ask")]
        public async Task<IActionResult> AskAI([FromBody] UserMessageDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var response = await _mediator.Send(new GetAIResponseQuery(dto));
            return Ok(new { reply = response });
        }
    }
}