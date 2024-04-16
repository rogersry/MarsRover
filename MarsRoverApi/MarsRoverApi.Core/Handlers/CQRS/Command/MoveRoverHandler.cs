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

    public class MoveRoverHandler : IRequestHandler<MoveRoverCommand, bool>
    {
        private readonly ILogger<MoveRoverHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IMessageSession _messageSession;

        public MoveRoverHandler(ILogger<MoveRoverHandler> logger, IMapper mapper, IMessageSession messageSession)
        {
            _mapper = mapper;
            _messageSession = messageSession;
            _logger = logger;
        }

        public async Task<bool> Handle(MoveRoverCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling MoveRoverCommand: {request}");

            try
            {
                var message = _mapper.Map<MoveRoverRequestMessage>(request);
                _logger.LogInformation($"Sending MoveRoverRequestMessage to Rover: {System.Text.Json.JsonSerializer.Serialize(message)}");
                await _messageSession.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Sending MoveRoverMessage to Rover: {ex.Message}");
                return false;
            }
            _logger.LogInformation($"MoveRoverMessage Sent");
            
            return true;
        }
    }
}
