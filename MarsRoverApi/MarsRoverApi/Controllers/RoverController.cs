using MarsRoverApi.Core.Handlers.CQRS.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MarsRoverApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoverController : ControllerBase
    {
        private readonly ILogger<RoverController> _logger;
        private readonly IMediator _mediator;

        public RoverController(ILogger<RoverController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("move")]
        public async Task<IActionResult> Move([FromBody] MoveRoverCommand request) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
