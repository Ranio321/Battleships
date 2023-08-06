using Battleships.Boards;
using Battleships.Boards.Models;
using Battleships.Models;
using Battleships.Services;
using Moq;

namespace Battleships.Tests.Services
{
    public class CoordinatesValidatorTests
    {
        private readonly CoordinatesValidator _sut;
        private readonly Mock<IBoard> _boardMock;

        public CoordinatesValidatorTests()
        {
            _boardMock = new Mock<IBoard>();
            _sut = new CoordinatesValidator(_boardMock.Object);
        }

        [Theory]
        [InlineData(25, 25)]
        [InlineData(25, 1)]
        [InlineData(1, 25)]
        [InlineData(26, 26)]
        public void ValidateCoordinates_CoordinatesOutOfRange_ReturnsOutOfBoundsError(int x, int y)
        {
            var coordinates = new Coordinates
            {
                X = x,
                Y = y,
            };
            _boardMock.Setup(x => x.GetSize()).Returns(25);

            var result = _sut.ValidateCoordinates(coordinates);

            Assert.Equal(CoordinatesValidationError.OutOfBounds, result);
        }

        [Fact]
        public void ValidateCoordinates_CellWithProvidedCoordinatesIsNotUnexplored_ReturnAlreadyUsedError()
        {
            var coordinates = new Coordinates
            {
                X = 1,
                Y = 24,
            };
            _boardMock.Setup(x => x.GetSize()).Returns(25);
            _boardMock.Setup(x => x.GetCellType(It.Is<Coordinates>(x => x.X == coordinates.X && x.Y == coordinates.Y))).Returns(CellType.Hit);

            var result = _sut.ValidateCoordinates(coordinates);

            Assert.Equal(CoordinatesValidationError.AlreadyUsed, result);
        }

        [Fact]
        public void ValidateCoordinates_WithValidCoordinates_ReturnsNull()
        {
            var coordinates = new Coordinates
            {
                X = 1,
                Y = 24,
            };
            _boardMock.Setup(x => x.GetSize()).Returns(25);
            _boardMock.Setup(x => x.GetCellType(It.Is<Coordinates>(x => x.X == coordinates.X && x.Y == coordinates.Y))).Returns(CellType.Unexplored);

            var result = _sut.ValidateCoordinates(coordinates);

            Assert.Null(result);
        }
    }
}
