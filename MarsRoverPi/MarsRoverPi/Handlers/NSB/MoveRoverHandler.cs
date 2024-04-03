using MarsRoverMessages;
using NServiceBus;
using System.Device.Gpio;

namespace MarsRoverPi.Handlers.NSB
{

    public class MoveRoverHandler : IHandleMessages<MoveRoverMessage>, IDisposable
    {
        private readonly int _motorPin1 = 17;
        private readonly int _motorPin2 = 18;
        private readonly int _motorEnable = 27;
        private readonly GpioController _controller;

        private readonly ILogger<MoveRoverHandler> _logger;

        public MoveRoverHandler(ILogger<MoveRoverHandler> logger)
        {
            _logger = logger;

            _controller = new GpioController();
            _controller.OpenPin(_motorPin1, PinMode.Output);
            _controller.OpenPin(_motorPin2, PinMode.Output);
            _controller.OpenPin(_motorEnable, PinMode.Output);

            _controller.Write(_motorEnable, PinValue.Low);
        }

        public void Dispose()
        {
            _controller.Dispose();
        }

        public Task Handle(MoveRoverMessage message, IMessageHandlerContext context)
        {
            _logger.LogInformation($"MoveRoverHandler - Received MoveRoverMessage.  Direction: {message.Direction}.  Milliseconds: {message.Milliseconds}");
            switch (message.Direction)
            {
                case Direction.Left:
                    break;
                case Direction.Right:
                    break;
                case Direction.Forward:
                    Forward(message.Milliseconds);
                    break;
                case Direction.Reverse:
                    Reverse(message.Milliseconds);
                    break;
            }
            _logger.LogInformation("MoveRoverHandler - Handled MoveRoverMessage.");

            return Task.CompletedTask;
        }

        private void Forward(int milliseconds)
        {
            _logger.LogInformation("Moving Forward");
            _controller.Write(_motorEnable, PinValue.High);
            _controller.Write(_motorPin1, PinValue.High);
            _controller.Write(_motorPin2, PinValue.Low);
            Thread.Sleep(milliseconds);
            _controller.Write(_motorEnable, PinValue.Low);
            _logger.LogInformation("Stopped");
        }

        private void Reverse(int milliseconds)
        {
            _logger.LogInformation("Moving Reverse");
            _controller.Write(_motorEnable, PinValue.High);
            _controller.Write(_motorPin1, PinValue.Low);
            _controller.Write(_motorPin2, PinValue.High);
            Thread.Sleep(milliseconds);
            _controller.Write(_motorEnable, PinValue.Low);
            _logger.LogInformation("Stopped");
        }
    }
}
