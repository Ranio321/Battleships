using System.Diagnostics.CodeAnalysis;
using Battleships.Models;

namespace Battleships.Boards.Models
{
    internal class Cell
    {
        public required Coordinates Coordinates { get; init; }
        public bool Occupied { get; private set; } = false;
        public CellType Type { get; private set; } = CellType.Unexplored;

        [SetsRequiredMembers]
        public Cell(int x, int y)
        {
            Coordinates = new()
            {
                X = x,
                Y = y
            };
        }

        public void SetAsOccupied()
            => Occupied = true;

        public void SetType(CellType type)
            => Type = type;
    }
}
