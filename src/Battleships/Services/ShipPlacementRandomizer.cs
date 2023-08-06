using Battleships.Ships;
using Battleships.Boards;
using Battleships.Models;
using Battleships.Services.Models;
using Battleships.Services.Abstraction;

namespace Battleships.Services
{
    public class ShipPlacementRandomizer : IShipPlacementService
    {
        private static readonly Random Random = new();

        private int BoardSize => _board.GetSize();
        private IBoard _board = null!;

        public void PlaceShips(IBoard boards, IEnumerable<Ship> ships)
        {
            _board = boards;
            RandomizePlacement(ships);
        }

        private void RandomizePlacement(IEnumerable<Ship> ships)
        {
            foreach (var ship in ships)
            {
                while (true)
                {
                    var plane = GetRandomizedPlane();
                    var shipLength = ship.Length;
                    Coordinates shipInitialCoordinates;

                    if (plane == Plane.Horizontal)
                    {
                        shipInitialCoordinates = new Coordinates
                        {
                            X = Random.Next(0, BoardSize - shipLength),
                            Y = Random.Next(0, BoardSize),
                        };
                    }
                    else
                    {
                        shipInitialCoordinates = new Coordinates
                        {
                            X = Random.Next(0, BoardSize),
                            Y = Random.Next(0, BoardSize - shipLength),
                        };

                    }

                    var shipPlacedSuccessfully = TryPlaceShip(ship, shipInitialCoordinates, plane);

                    if (shipPlacedSuccessfully)
                    {
                        break;
                    }
                }
            }
        }

        private bool TryPlaceShip(Ship ship, Coordinates coordinates, Plane plane)
        {
            return plane switch
            {
                Plane.Horizontal => PlaceShipHorizontally(ship, coordinates),
                Plane.Vertical => PlaceShipVertically(ship, coordinates),
                _ => throw new NotImplementedException()
            };
        }

        private bool PlaceShipHorizontally(Ship ship, Coordinates coordinates)
        {
            var shipLength = ship.Length;
            var xPosition = coordinates.X;
            var yPosition = coordinates.Y;

            if (xPosition + shipLength > BoardSize)
            {
                return false;
            }

            for (var i = 0; i < shipLength; i++)
            {
                var cellOcuppied = IsCellOccupied(xPosition + i, yPosition);
                if (cellOcuppied)
                {
                    return false;
                }
            }

            var shipCoordinates = new List<Coordinates>();
            for (var i = 0; i < shipLength; i++)
            {
                var newXPosition = xPosition + i;
                SetCellAsOccupied(newXPosition, yPosition);
                shipCoordinates.Add(new Coordinates(newXPosition, yPosition));
            }

            ship.SetLocation(shipCoordinates);
            return true;
        }

        private bool PlaceShipVertically(Ship ship, Coordinates coordinates)
        {
            var shipLength = ship.Length;
            var xPosition = coordinates.X;
            var yPosition = coordinates.Y;

            if (yPosition + shipLength > BoardSize)
            {
                return false;
            }

            for (var i = 0; i < shipLength; i++)
            {
                var cellOcuppied = IsCellOccupied(xPosition, yPosition + i);
                if (cellOcuppied)
                {
                    return false;
                }
            }

            var shipCoordinates = new List<Coordinates>();
            for (var i = 0; i < shipLength; i++)
            {
                var newYPosition = yPosition + i;
                SetCellAsOccupied(xPosition, newYPosition);
                shipCoordinates.Add(new Coordinates(xPosition, newYPosition));
            }

            ship.SetLocation(shipCoordinates);
            return true;
        }

        private bool IsCellOccupied(int x, int y)
            => _board.IsCellOccupied(new Coordinates(x, y));

        private void SetCellAsOccupied(int x, int y)
            => _board.SetCellAsOccupied(new Coordinates(x, y));

        private static Plane GetRandomizedPlane()
            => Random.Next(0, 2) == 0 ? Plane.Horizontal : Plane.Vertical;
    }
}
