using Battleships.Boards.Models;
using Battleships.Models;

namespace Battleships.Boards
{
    public interface IBoard
    {
        int GetSize();
        CellType GetCellType(Coordinates coordinates);
        bool IsCellOccupied(Coordinates coordinates);
        void SetCellType(Coordinates coordinates, CellType cellType);
        void SetCellAsOccupied(Coordinates coordinates);
        CoordinatesValidationError? ValidateCoordinates(Coordinates coordinates);
    }
}
