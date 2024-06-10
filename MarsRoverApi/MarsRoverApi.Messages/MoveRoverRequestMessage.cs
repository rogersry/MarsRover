using NServiceBus;

namespace MarsRoverApi.Messages
{
    public class MoveRoverRequestMessage : IMessage
    {
        public Direction Direction { get; set; }
        public int Milliseconds { get; set; }
    }

    public enum Direction
    {
        Forward, Reverse, Left, Right
    }
}
