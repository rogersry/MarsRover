using MarsRoverApi.Core.Handlers.CQRS.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MarsRoverApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoverController(ILogger<RoverController> logger, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<RoverController> _logger = logger;
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [Route("move")]
        public async Task<IActionResult> Move([FromBody] MoveRoverCommand request) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Calling MoveRoverCommandHandler");
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
