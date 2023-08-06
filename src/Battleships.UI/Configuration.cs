using Battleships.Ships;

namespace Battleships.UI
{
    internal static class Configuration
    {
        public const int BoardSize = 10;

        public static readonly ShipType[] Ships = new ShipType[]
        {
            ShipType.Destroyer,
            ShipType.Destroyer,
            ShipType.Battleship
        };

        public static List<Ship> GetShips()
        {
            var ships = new List<Ship>();
            foreach (var shipType in Ships)
            {
                ships.Add(Ship.Create(shipType));
            }

            return ships;
        }
    }
}
