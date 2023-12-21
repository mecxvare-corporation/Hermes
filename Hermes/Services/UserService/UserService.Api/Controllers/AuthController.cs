using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Auth.Queries;
using UserService.Application.Dtos;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _mediator;

        public AuthController(ISender mediator)
        {
            _mediator = mediator;
        }

        //TODO will be removed in future
        [HttpGet]
        public async Task<ActionResult<UserMinimalInfoDto>> GetAuthorizedUserAsync()
        {
            return Ok(await _mediator.Send(new GetAuthorizedUserQuery()));
        }
    }
}
