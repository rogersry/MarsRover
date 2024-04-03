using NServiceBus;

namespace MarsRoverMessages
{
    public class MoveRoverMessage : IMessage
    {
        public Direction Direction { get; set; }
        public int Milliseconds { get; set; }

    }
    public enum Direction
    {
        Forward, Reverse, Left, Right
    }

}
