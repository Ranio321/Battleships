using Battleships.Boards;
using Battleships.Boards.Models;
using Battleships.Exceptions;
using Battleships.Models;

namespace Battleships.Tests
{
    public class BoardTests
    {
        private readonly Board _sut;

        public BoardTests()
        {
            _sut = new Board(25);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(25)]
        [InlineData(5)]
        public void GetSize_Always_ReturnsCorrectBoardSize(int size)
        {
            var board = new Board(size);

            var actualBoardSize = board.GetSize();

            Assert.Equal(size, actualBoardSize);
        }

        [Fact]
        public void SetCellAsOccupied_WithCorrectCoordinates_SetsCorrectCellAsOccupied()
        {
            var coordinates = new Coordinates
            {
                X = 1,
                Y = 2,
            };

            _sut.SetCellAsOccupied(coordinates);

            var isOccupied = _sut.IsCellOccupied(coordinates);
            Assert.True(isOccupied);
        }

        [Fact]
        public void SetCellAsOccupied_WthInvalidCoordinates_ThrowsException()
        {
            var coordinates = new Coordinates
            {
                X = 26,
                Y = 2,
            };

            var action = () => _sut.SetCellAsOccupied(coordinates);

            Assert.Throws<InvalidCoordinatesException>(action);
        }

        [Theory]
        [InlineData(1, 1, CellType.Missed)]
        [InlineData(10, 10, CellType.Hit)]
        [InlineData(11, 1, CellType.Unexplored)]
        [InlineData(24, 24, CellType.Destroyed)]
        public void SetCellType_WithValidCoordinates_SetsCorrectCellType(int x, int y, CellType cellType)
        {
            var coordinates = new Coordinates
            {
                X = x,
                Y = y,
            };

            _sut.SetCellType(coordinates, cellType);

            var actualCellType = _sut.GetCellType(coordinates);
            Assert.Equal(cellType, actualCellType);
        }

        [Fact]
        public void SetCellType_WithInvalidCoordinates_ThrowsException()
        {
            var coordinates = new Coordinates
            {
                X = 25,
                Y = 1,
            };

            var action = () => _sut.SetCellType(coordinates, CellType.Hit);

            Assert.Throws<InvalidCoordinatesException>(action);
        }
    }
}