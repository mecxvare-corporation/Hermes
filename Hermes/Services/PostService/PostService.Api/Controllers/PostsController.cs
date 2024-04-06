using MediatR;

using Microsoft.AspNetCore.Mvc;

using PostService.Application.Dtos;
using PostService.Application.Posts.Commands;
using PostService.Application.Posts.Queries;

namespace PostService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ISender _mediator;

        public PostsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPostAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetPostQuery(id)));
        }

        [HttpPost(Name = nameof(CreatePost))]
        public async Task<ActionResult> CreatePost([FromBody] CreatePostCommand command)
        {
            var postId = await _mediator.Send(command);

            return Ok(postId);
        }
    }
}
