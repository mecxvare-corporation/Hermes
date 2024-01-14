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
            //// Testing
            //// Getting token for client
            //var client = new HttpClient();
            //var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            //if (disco.IsError)
            //{
            //    Console.WriteLine(disco.Error);

            //    return BadRequest();
            //}

            //// request token
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            //{
            //    Address = disco.TokenEndpoint,

            //    ClientId = "userservice",
            //    ClientSecret = "admin1234",
            //    Scope = "identityprovider",
            //});

            //if (tokenResponse.IsError)
            //{
            //    Console.WriteLine(tokenResponse.Error);
            //    return BadRequest();
            //}

            //Console.WriteLine(tokenResponse.AccessToken);

            return Ok(await _mediator.Send(new GetAuthorizedUserQuery()));
        }
    }
}
