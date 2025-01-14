namespace OOD_Assignment
{
    public interface IEntity
    {
        int X { get; }
        int Y { get; }
    }

    public abstract class Entity : IEntity
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }

        protected Entity(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Guard : Entity
    {
        public Guard(int x, int y) : base(x, y) { }
    }

    public class Fence : Entity
    {
        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int EndX { get; private set; }
        public int EndY { get; private set; }

        public Fence(int startX, int startY, int endX, int endY) : base((startX + endX) / 2, (startY + endY) / 2)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
        }
    }

    public class Sensor : Entity
    {
        public double Range { get; private set; }

        public Sensor(int x, int y, double range) : base(x, y)
        {
            Range = range;
        }
    }

    public class Camera : Entity
    {
        public char Facing { get; private set; }

        public Camera(int x, int y, char facing) : base(x, y)
        {

            Facing = facing;
        }
    }
}
