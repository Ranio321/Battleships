using Battleships.Boards.Models;
using Battleships.Exceptions;
using Battleships.Models;

namespace Battleships.Boards
{
    public class Board : IBoard
    {
        private readonly Cell[,] _cells;
        private readonly int _size;

        public Board(int size)
        {
            _size = size;
            _cells = new Cell[size, size];
            InitializeCells(size);
        }

        public CellType GetCellType(Coordinates coordinates)
            => GetCell(coordinates).Type;

        public int GetSize()
            => _size;

        public void SetCellAsOccupied(Coordinates coordinates)
            => GetCell(coordinates).SetAsOccupied();

        public bool IsCellOccupied(Coordinates coordinates)
            => GetCell(coordinates).Occupied;

        public void SetCellType(Coordinates coordinates, CellType cellType)
            => GetCell(coordinates).SetType(cellType);

        public CoordinatesValidationError? ValidateCoordinates(Coordinates coordinates)
        {
            if (coordinates.X >= _size || coordinates.Y >= _size)
                return CoordinatesValidationError.OutOfBounds;

            if (_cells[coordinates.X, coordinates.Y].Type != CellType.Unexplored)
                return CoordinatesValidationError.AlreadyUsed;

            return null;
        }

        private Cell GetCell(Coordinates coordinates)
        {
            if (coordinates.X >= _size || coordinates.Y >= _size)
                throw new InvalidCoordinatesException(coordinates);

            return _cells[coordinates.X, coordinates.Y];
        }

        private void InitializeCells(int size)
        {
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    _cells[i,j] = new Cell(i, j);
                }
            }
        }
    }
}
