using NServiceBus;

namespace MarsRoverApi.Messages
{
    public class MoveRoverResponseMessage : IMessage
    {
        public string ResponseMessage { get; private set; }
        public DateTime ResponseDateTime { get; private set; }

        public MoveRoverResponseMessage(string responseMessage, DateTime responseDateTime)
        {
            ResponseMessage = responseMessage;
            ResponseDateTime = responseDateTime;
        }
    }
}
