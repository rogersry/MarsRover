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
        [Required]
        [EnumDataType(typeof(Direction))]
        public Direction Direction { get; set; }

        [Required]
        [Range(-10000, 10000)]
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
            var message = _mapper.Map<MoveRoverMessage>(request);

            _logger.LogInformation($"MoveRoverHandler - Sending MoveRoverMessage to Rover.  Direction: {request.Direction}.  Milliseconds: {request.Milliseconds}");
            await _messageSession.Send(message);
            _logger.LogInformation($"MoveRoverHandler - MoveRoverMessage Sent");
            
            return true;
        }
    }
}
