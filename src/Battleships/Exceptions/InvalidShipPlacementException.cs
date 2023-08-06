namespace Battleships.Exceptions
{
    public class InvalidShipPlacementException : Exception
    {
        public InvalidShipPlacementException(int shipLength)
            : base($"Coordinates must have {shipLength} length")
        {
            
        }
    }
}
