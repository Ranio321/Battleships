using Battleships.Boards;
using Battleships.Models;
using Battleships.Services;
using Battleships.Ships;

namespace Battleships.Tests.Services
{
    public class ShipPlacementRandomizerTests
    {
        private int BoardSize => _board.GetSize();

        private readonly ShipPlacementRandomizer _sut;
        private readonly IBoard _board;

        public ShipPlacementRandomizerTests()
        {
            _board = new Board(10);
            _sut = new ShipPlacementRandomizer();
        }

        [Theory]
        [MemberData(nameof(ShipsData))]
        public void RandomizePlacement_ValidNumberOfShips_CorrectlyPlacesShips(Ship[] ships)
        {
            var shipsLength = ships
                .Select(x => x.Length)
                .Aggregate((x, y) => x + y);

            _sut.PlaceShips(_board, ships);

            var numberOfOccupiedCells = GetNumberOfOccupiedCells();
            Assert.Equal(numberOfOccupiedCells, shipsLength);
        }

        public static IEnumerable<object[]> ShipsData()
        {
            yield return new object[]
            {
                new Ship[]
                {
                    Ship.Create(ShipType.Battleship),
                }
            };
            yield return new object[]
            {
                new Ship[]
                {
                    Ship.Create(ShipType.Destroyer),
                }
            };
            yield return new object[]
            {
                new Ship[]
                {
                    Ship.Create(ShipType.Battleship),
                    Ship.Create(ShipType.Destroyer),
                    Ship.Create(ShipType.Destroyer)
                }
            };
            yield return new object[]
            {
                new Ship[]
                {
                    Ship.Create(ShipType.Battleship),
                    Ship.Create(ShipType.Destroyer),
                    Ship.Create(ShipType.Destroyer)
                }
            };
            yield return new object[]
            {
                new Ship[]
                {
                    Ship.Create(ShipType.Battleship),
                    Ship.Create(ShipType.Destroyer),
                    Ship.Create(ShipType.Battleship)
                }
            };
        }

        private int GetNumberOfOccupiedCells()
        {
            var numberOfOccupiedCells = 0;

            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    var coordinats = new Coordinates
                    {
                        X = i,
                        Y = j
                    };

                    var cellOccupied = _board.IsCellOccupied(coordinats);

                    if (cellOccupied)
                        numberOfOccupiedCells++;
                }
            }
            return numberOfOccupiedCells;
        }

    }
}