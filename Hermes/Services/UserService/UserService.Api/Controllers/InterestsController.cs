using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Interests.Command;
using UserService.Application.Interests.Query;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestsController : ControllerBase
    {
        private readonly ISender _mediator;

        public InterestsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = nameof(GetAllInterests))]
        public async Task<ActionResult> GetAllInterests()
        {
            var interests = await _mediator.Send(new GetAllInterestsQuery());

            return Ok(interests);
        }

        [HttpPost(Name = nameof(CreateInterest))]
        public async Task<ActionResult> CreateInterest([FromBody] CreateInterestCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)
            {
                return RedirectToAction(nameof(GetAllInterests));
            }
            else
                return BadRequest();
        }

        [HttpDelete(Name = nameof(DeleteInterest))]
        public async Task<ActionResult> DeleteInterest([FromBody] DeleteInterestCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)
            {
                return RedirectToAction(nameof(GetAllInterests));
            }
            else
                return BadRequest();
        }
    }
}
