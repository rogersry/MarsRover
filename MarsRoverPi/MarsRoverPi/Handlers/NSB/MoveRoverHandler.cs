using Azure.Core;
using MarsRoverMessages;
using Microsoft.Azure.Amqp.Transaction;
using NServiceBus;
using System.Device.Gpio;

namespace MarsRoverPi.Handlers.NSB
{

    public class MoveRoverHandler : IHandleMessages<MoveRoverRequestMessage>, IDisposable
    {
        private readonly int _motorLeftPin1 = 17;
        private readonly int _motorLeftPin2 = 18;
        private readonly int _motorLeftEnable = 27;
        private readonly Motor _leftMotor;
        private readonly Motor _rightMotor;

        private readonly int _motorRightPin1 = 21;
        private readonly int _motorRightPin2 = 16;
        private readonly int _motorRightEnable = 25;

        private readonly GpioController _controller;

        private readonly ILogger<MoveRoverHandler> _logger;

        public MoveRoverHandler(ILogger<MoveRoverHandler> logger)
        {
            _logger = logger;
            _controller = new GpioController();
            _leftMotor = new Motor(_motorLeftPin1, _motorLeftPin2, _motorLeftEnable, _controller);
            _rightMotor = new Motor(_motorRightPin1, _motorRightPin2, _motorRightEnable, _controller);
            _controller.Write(_motorLeftEnable, PinValue.Low);
        }

        public void Dispose()
        {
            _controller.Dispose();
        }

        public async Task Handle(MoveRoverRequestMessage message, IMessageHandlerContext context)
        {
            var messageJsonString = System.Text.Json.JsonSerializer.Serialize(message);
            _logger.LogInformation($"MoveRoverHandler - Received MoveRoverMessage.  {messageJsonString}");
            try
            {
                foreach(var step in message.MoveRoverRequestMessageSteps)
                {
                    switch (step.Direction)
                    {
                        case Direction.Left:
                            _logger.LogInformation("Turning Left");
                            Parallel.Invoke(
                                () =>
                                {
                                    _leftMotor.Reverse(step.Milliseconds);
                                },
                                () =>
                                {
                                    _rightMotor.Forward(step.Milliseconds);
                                }
                            );
                            break;
                        case Direction.Right:
                            _logger.LogInformation("Turning Right");
                            Parallel.Invoke(
                                () =>
                                {
                                    _leftMotor.Forward(step.Milliseconds);
                                },
                                () =>
                                {
                                    _rightMotor.Reverse(step.Milliseconds);
                                }
                            );
                            break;
                        case Direction.Forward:
                            _logger.LogInformation("Moving Forward");
                            Parallel.Invoke(
                                () =>
                                {
                                    _leftMotor.Forward(step.Milliseconds);
                                },
                                () =>
                                {
                                    _rightMotor.Forward(step.Milliseconds);
                                }
                            );
                            break;
                        case Direction.Reverse:
                            _logger.LogInformation("Moving Reverse");
                            Parallel.Invoke(
                                () =>
                                {
                                    _leftMotor.Reverse(step.Milliseconds);
                                },
                                () =>
                                {
                                    _rightMotor.Reverse(step.Milliseconds);
                                }
                            );
                            break;
                    }
                }
                
                _logger.LogInformation("MoveRoverHandler - Handled MoveRoverMessage.");
            }
            catch (Exception ex)
            {
                var errorMessage = $"MoveRoverHandler - {ex.Message}";
                var roverErrorMessage = new RoverErrorMessage(errorMessage, DateTime.Now);
                _logger.LogError(errorMessage);
                await context.Send(roverErrorMessage);
                return;
            }
            var moveRoverResponseMessage = new MoveRoverResponseMessage($"Rover move complete {messageJsonString}", DateTime.Now);
            await context.Send(moveRoverResponseMessage);
        }
    }

    internal class Motor
    {
        private readonly int _motorPin1;
        private readonly int _motorPin2;
        private readonly int _motorEnable;
        private readonly GpioController _controller;

        public Motor(int pin1, int pin2, int pinEnable, GpioController controller)
        {
            _motorPin1 = pin1;
            _motorPin2 = pin2;
            _motorEnable = pinEnable;
            _controller = controller;
            _controller.OpenPin(_motorPin1, PinMode.Output);
            _controller.OpenPin(_motorPin2, PinMode.Output);
            _controller.OpenPin(_motorEnable, PinMode.Output);
        }

        public void Forward(int milliseconds)
        {
            _controller.Write(_motorEnable, PinValue.High);
            _controller.Write(_motorPin1, PinValue.High);
            _controller.Write(_motorPin2, PinValue.Low);
            Thread.Sleep(milliseconds);
            _controller.Write(_motorEnable, PinValue.Low);
        }

        public void Reverse(int milliseconds)
        {
            _controller.Write(_motorEnable, PinValue.High);
            _controller.Write(_motorPin1, PinValue.Low);
            _controller.Write(_motorPin2, PinValue.High);
            Thread.Sleep(milliseconds);
            _controller.Write(_motorEnable, PinValue.Low);
        }
    }
}
