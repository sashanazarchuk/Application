using EventSystem.Application.Queries.Tags.GetAllTags;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventSystem.API.Controllers
{
    [Route("tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TagController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        /// <summary>
        /// Fetch all tags
        /// </summary>
        /// <remarks>
        /// This endpoint retrieves all available tags in the system.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> FetchAllTags()
        {
            var tag = await _mediator.Send(new GetAllTagsQuery());
            return Ok(tag);
        }
    }
}