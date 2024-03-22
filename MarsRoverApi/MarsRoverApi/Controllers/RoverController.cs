using MarsRoverApi.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarsRoverApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoverController : ControllerBase
    {
        private readonly ILogger<RoverController> _logger;

        public RoverController(ILogger<RoverController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("status")]
        public IActionResult GetStatus()
        {
            return Ok("Ready");
        }

        [HttpGet]
        [Route("history")]
        public IActionResult GetHistory()
        {
            return Ok("History");
        }

        [HttpPost]
        [Route("move")]
        public IActionResult Move([FromBody] MoveRequest request) {
            return Ok();
        }
    }
}
