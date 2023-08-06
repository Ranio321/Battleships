using Battleships.Exceptions;
using Battleships.Models;

namespace Battleships.Ships
{
    public class Ship
    {
        public int Length { get; init; }
        public int HitsReceived { get; private set; }
        public bool Destroyed => HitsReceived == Length;

        private readonly List<Coordinates> _locations = new();

        private Ship(int length)
        {
            Length = length;
        }

        public static Ship Create(ShipType type)
            => type switch
            {
                ShipType.Destroyer => new Ship(5),
                ShipType.Battleship => new Ship(4),
                _ => throw new NotImplementedException(),
            };

        public void ReceiveHit()
        {
            if (HitsReceived < Length)
            {
                HitsReceived++;
            }
        }

        public void SetLocation(IEnumerable<Coordinates> coordinates)
        {
            if (coordinates.Count() != Length)
                throw new InvalidShipPlacementException(Length);

            if (!_locations.Any())
            {
                _locations.AddRange(coordinates);
            }
        }

        public IReadOnlyList<Coordinates> GetShipLocation()
            => _locations.AsReadOnly();
    }
}
