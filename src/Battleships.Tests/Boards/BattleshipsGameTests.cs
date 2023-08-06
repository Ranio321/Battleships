using Battleships.Boards;
using Battleships.Boards.Models;
using Battleships.Exceptions;
using Battleships.Models;
using Battleships.Services.Abstraction;
using Battleships.Ships;
using Moq;

namespace Battleships.Tests.Boards
{
    public class BattleshipsGameTests
    {
        private readonly BattleshipsGame _sut;
        private readonly Mock<IBoard> _boardMock;
        private readonly Mock<ICoordinatesValidator> _coordinatesValidatorMock;

        public BattleshipsGameTests()
        {
            var shipPlacementServiceMock = new Mock<IShipPlacementService>();

            _coordinatesValidatorMock = new Mock<ICoordinatesValidator>();
            _boardMock = new Mock<IBoard>();

            _sut = new BattleshipsGame(_boardMock.Object, shipPlacementServiceMock.Object, _coordinatesValidatorMock.Object);
        }

        [Fact]
        public void BattleshipsGame_NoActionsTaken_IsInPlacementPhase()
        {
            var currentGameState = _sut.GameState;

            Assert.Equal(GameState.PlacementPhase, currentGameState);
        }

        [Fact]
        public void PlaceShips_AnyArray_ChangesGameStateToInProgress()
        {
            _sut.PlaceShips(Array.Empty<Ship>());

            var currentGameState = _sut.GameState;

            Assert.Equal(GameState.InProgress, currentGameState);
        }

        [Fact]
        public void Shoot_PlaceShipsNotCalled_ThrowsInvalidGameStateException()
        {
            var action = () => { _sut.Shoot(new Coordinates(1, 1)); };

            Assert.Throws<InvalidGameStateException>(action);
        }

        [Fact]
        public void Shoot_InCorrectGameStateAndOutOfBoundsCoordinatesPassed_ThrowsInvalidCoordinatesException()
        {
            var coordinates = new Coordinates(1, 1);
            _sut.PlaceShips(Array.Empty<Ship>());
            _coordinatesValidatorMock.Setup(x => x.ValidateCoordinates(coordinates)).Returns(CoordinatesValidationError.OutOfBounds);

            var action = () => _sut.Shoot(coordinates);

            Assert.Throws<InvalidCoordinatesException>(action);
        }

        [Fact]
        public void Shoot_InCorrectGameStateAndAlreadyUsedCoordinatesPassed_ThrowsInvalidCoordinatesException()
        {
            var coordinates = new Coordinates(1, 1);
            _sut.PlaceShips(Array.Empty<Ship>());
            _coordinatesValidatorMock.Setup(x => x.ValidateCoordinates(coordinates)).Returns(CoordinatesValidationError.AlreadyUsed);

            var action = () => _sut.Shoot(coordinates);

            Assert.Throws<InvalidCoordinatesException>(action);
        }

        [Fact]
        public void Shoot_NoShips_SetsGameStateToFinished()
        {
            _sut.PlaceShips(Array.Empty<Ship>());
            var coordinates = new Coordinates(1, 1);

            _sut.Shoot(coordinates);

            var currentGameState = _sut.GameState;

            Assert.Equal(GameState.Finished, currentGameState);
        }

        [Fact]
        public void Shoot_InCorrectGameStateAndShipHasBeenHit_SetsCellAsHit()
        {
            var ships = GetDummyShips();

            var coordinates = new Coordinates(1, 0);
            _sut.PlaceShips(ships);

            _boardMock.Verify(x => x.SetCellType(It.Is<Coordinates>(x => x.X == 1 && x.Y == 0), CellType.Hit), Times.Once);
        }


        [Fact]
        public void Shoot_InCorrectGameStateAndShipHasBeenHitTwoTimes_SetsHitsReceivedToTwo()
        {
            var ships = GetDummyShips();

            var coordinates = new Coordinates[]
            {
                new Coordinates(1, 0),
                new Coordinates(0, 0),
            };
            _sut.PlaceShips(ships);

            foreach (var coords in coordinates)
            {
                _sut.Shoot(coords);
            }

            Assert.Equal(2, ships[0].HitsReceived);
        }

        [Fact]
        public void Shoot_InCorrectGameStateAndShipHasBeenDestroyed_SetsCorrectCellsAsDestroyed()
        {
            var ships = GetDummyShips();

            _sut.PlaceShips(ships);
            var shipLocation = ships[0].GetShipLocation();

            foreach (var coordinates in shipLocation)
            {
                _sut.Shoot(coordinates);
            }

            foreach (var coordinates in shipLocation)
            {
                _boardMock.Verify(x => x.SetCellType(It.Is<Coordinates>(x => x.X == coordinates.X && x.Y == coordinates.Y), CellType.Destroyed), Times.Once);
            }
        }


        [Fact]
        public void Shoot_InCorrectGameStateAndShipHasNotBeenHit_SetsCellAsMiss()
        {
            var ships = GetDummyShips();

            var coordinates = new Coordinates(0, 1);
            _sut.PlaceShips(ships);

            _boardMock.Verify(x => x.SetCellType(It.Is<Coordinates>(x => x.X == 0 && x.Y == 1), CellType.Missed), Times.Once);
        }

        private static Ship[] GetDummyShips()
        {
            var ship = Ship.Create(ShipType.Battleship);
            var shipLocation = new List<Coordinates>();

            for (int i = 0; i < ship.Length; i++)
            {
                shipLocation.Add(new Coordinates(i, 0));
            }
            ship.SetLocation(shipLocation);

            var ships = new Ship[] { ship };

            return ships;
        }
    }
}
