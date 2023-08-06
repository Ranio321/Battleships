using Battleships.Boards;
using Battleships.Boards.Models;
using Battleships.Models;
using Battleships.Services.Abstraction;

namespace Battleships.Services
{
    public class CoordinatesValidator : ICoordinatesValidator
    {
        private int BoardSize => _board.GetSize();

        private readonly IBoard _board;

        public CoordinatesValidator(IBoard board)
        {
            _board = board;
        }

        public CoordinatesValidationError? ValidateCoordinates(Coordinates coordinates)
        {
            if (coordinates.X >= BoardSize || coordinates.Y >= BoardSize)
                return CoordinatesValidationError.OutOfBounds;

            if (_board.GetCellType(coordinates) != CellType.Unexplored)
                return CoordinatesValidationError.AlreadyUsed;

            return null;
        }
    }
}
