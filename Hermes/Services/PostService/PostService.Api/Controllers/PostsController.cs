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

        [HttpGet("post/{id}")]
        public async Task<ActionResult<PostDto>> GetPostAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetPostQuery(id)));
        }

        [HttpGet("posts",Name = "GetAllPosts")]
        public async Task<ActionResult<List<PostDto>>> GetPostsAsync()
        {
            return Ok(await _mediator.Send(new GetPostsQuery()));
        }

        [HttpGet("user/{userId}/posts", Name = "GetUserPosts")]
        public async Task<ActionResult<List<PostDto>>> GetUserPostsAsync(Guid userId)
        {
            return Ok(await _mediator.Send(new GetUserPostsQuery(userId)));
        }

        [HttpPost("create", Name = nameof(CreatePostAsync))]
        public async Task<ActionResult> CreatePostAsync([FromBody] CreatePostCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("update",Name = nameof(Update))]
        public async Task<ActionResult> Update([FromBody] UpdatePostCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("delete/{id}", Name = nameof(Delete))]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeletePostCommand(id));

            return NoContent();
        }
    }
}
