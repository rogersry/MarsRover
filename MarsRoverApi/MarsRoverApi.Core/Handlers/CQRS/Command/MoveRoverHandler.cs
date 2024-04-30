using AutoMapper;
using MarsRoverMessages;
using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.ComponentModel.DataAnnotations;

namespace MarsRoverApi.Core.Handlers.CQRS.Command
{
    public record MoveRoverCommand : IRequest<bool>
    {
        [Required]
        [EnumDataType(typeof(Direction))]
        public Direction Direction { get; set; }

        [Required]
        [Range(0, 10000)]
        public int Milliseconds { get; set; }
    }

    public enum Direction
    {
        Forward, Reverse, Left, Right
    }

    public class MoveRoverHandler(ILogger<MoveRoverHandler> logger, IMapper mapper, IMessageSession messageSession) : IRequestHandler<MoveRoverCommand, bool>
    {
        private readonly ILogger<MoveRoverHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageSession _messageSession = messageSession;

        public async Task<bool> Handle(MoveRoverCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling MoveRoverCommand: {request}", request);

            try
            {
                var message = _mapper.Map<MoveRoverRequestMessage>(request);
                _logger.LogInformation("Sending MoveRoverRequestMessage to Rover: {message}", System.Text.Json.JsonSerializer.Serialize(message));
                await _messageSession.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Sending MoveRoverMessage to Rover: {errorMessage}", ex.Message);
                return false;
            }
            _logger.LogInformation($"MoveRoverMessage Sent");
            
            return true;
        }
    }
}
