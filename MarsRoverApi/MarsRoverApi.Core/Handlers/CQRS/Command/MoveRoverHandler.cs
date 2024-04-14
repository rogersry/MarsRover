using AutoMapper;
using MarsRoverMessages;
using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.ComponentModel.DataAnnotations;

namespace MarsRoverApi.Core.Handlers.CQRS.Command
{
    public class MoveRoverCommand : IRequest<bool>
    {
        public List<MoveRoverCommandStep> MoveRoverCommandSteps { get; set; }
        public MoveRoverCommand()
        {
            MoveRoverCommandSteps = new List<MoveRoverCommandStep>();
        }
    }

    public class MoveRoverCommandStep
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
            var requestJsonString = System.Text.Json.JsonSerializer.Serialize(request);
            _logger.LogInformation($"MoveRoverHandler - Sending MoveRoverMessage to Rover.  {requestJsonString}");
            
            try
            {
                var message = new MoveRoverRequestMessage();

                foreach (var step in request.MoveRoverCommandSteps)
                {
                    var moveRoverRequestMessageStep = _mapper.Map<MoveRoverRequestMessageStep>(step);
                    message.MoveRoverRequestMessageSteps.Add(moveRoverRequestMessageStep);
                }
            
                await _messageSession.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"MoveRoverHandler - Error: {ex.Message}");
                return false;
            }
            _logger.LogInformation($"MoveRoverHandler - MoveRoverMessage Sent");
            
            return true;
        }
    }
}
