namespace MarsRoverApi.Requests
{
    public class MoveRequest
    {
        public Direction Direction { get; set; }
        public int Spaces { get; set; }
    }

    public enum Direction
    {
        Up, Down, Left, Right
    }
}
