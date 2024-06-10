using NServiceBus;

namespace MarsRoverApi.Messages
{
    public class RoverErrorMessage : IMessage
    {
        public string ErrorMessage { get; private set; }
        public DateTime ErrorDateTime { get; private set; }

        public RoverErrorMessage(string errorMessage, DateTime errorDateTime)
        {
            ErrorMessage = errorMessage;
            ErrorDateTime = errorDateTime;
        }
    }
}
