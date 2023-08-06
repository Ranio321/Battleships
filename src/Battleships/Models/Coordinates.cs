namespace Battleships.Models
{
    public class Coordinates
    {
        public int X { get; init; }
        public int Y { get; init; }

        public Coordinates()
        { }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
