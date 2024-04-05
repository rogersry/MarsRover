using NServiceBus;
using System;

namespace MarsRoverMessages
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
