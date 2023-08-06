using Battleships.Models;

namespace Battleships.Exceptions
{
    public class InvalidCoordinatesException : Exception
    {
        public InvalidCoordinatesException(Coordinates coordinates) 
            : base($"Provided coordinates are not valid (x: {coordinates.X}, y: {coordinates.Y})")
        {
        }
    }
}
