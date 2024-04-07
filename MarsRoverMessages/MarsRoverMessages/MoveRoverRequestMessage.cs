using NServiceBus;
using System.Collections.Generic;

namespace MarsRoverMessages
{

    public class MoveRoverRequestMessage : IMessage
    {
        public List<MoveRoverRequestMessageStep> MoveRoverRequestMessageSteps { get; set; }
        public MoveRoverRequestMessage()
        {
            MoveRoverRequestMessageSteps = new List<MoveRoverRequestMessageStep>();
        }
    }

    public class MoveRoverRequestMessageStep
    {
        public Direction Direction { get; set; }
        public int Milliseconds { get; set; }
    }

    public enum Direction
    {
        Forward, Reverse, Left, Right
    }

}
