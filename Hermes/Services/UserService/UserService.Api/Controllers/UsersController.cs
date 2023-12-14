﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Dtos;
using UserService.Application.Users.Commands;
using UserService.Application.Users.Queries;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _mediator;

        public UsersController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<ActionResult<List<UserDto>>> GetUsersAsync()
        {
            var usersDtos = await _mediator.Send(new GetUsersQuery());

            return Ok(usersDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetUserQuery(id)));
        }

        [HttpPost(Name = nameof(Create))]
        public async Task<ActionResult> Create([FromBody] CreateUserCommand command)
        {
            var newUserId = await _mediator.Send(command);

            return Ok(newUserId);
        }

        [HttpPut(Name = nameof(Update))]
        public async Task<ActionResult> Update([FromBody] UpdateUserCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}", Name = nameof(Delete))]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));

            return NoContent();
        }

        [HttpPut("interests")]
        public async Task<ActionResult> UpdateUserInterests([FromBody] UpdateUserInterestCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        [HttpDelete("interests")]
        public async Task<ActionResult> RemoveUserInterests([FromBody] DeleteUserInterestCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet("interests/{id}", Name = nameof(GetUserInterests))]
        public async Task<ActionResult<UserInterestsDto>> GetUserInterests(Guid id)
        {
            return Ok(await _mediator.Send(new GetUserInterestsQuery(id)));
        }

        [HttpPost("image/{userId}", Name =nameof(UploadImage))]
        public async Task<IActionResult> UploadImage([FromRoute] Guid userId, [FromForm] IFormFile imageFile)
        {
            // Convert the uploaded image file to a byte array
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                byte[] imageData = memoryStream.ToArray();

                // Get the content type of the image
                string imageContentType = imageFile.ContentType;

                // Send the image upload command to the MediatR pipeline
                var imageUrl = await _mediator.Send(new UploadUserProfilePictureCommand(userId, imageData, imageContentType));

                // Return the image URL in the response
                return Ok(new { ImageUrl = imageUrl });
            }
        }

        [HttpDelete("image/{userId}", Name = nameof(DeleteImage))]
        public async Task<IActionResult> DeleteImage(Guid userId)
        {
            await _mediator.Send(new DeleteUserProfileImageCommand(userId));
            return Ok();
        }
    }
}