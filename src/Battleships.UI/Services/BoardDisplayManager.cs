using Battleships.Boards;
using Battleships.Models;
using Battleships.Boards.Models;
using Battleships.UI.Models;
using Battleships.UI.Services.Abstraction;

namespace Battleships.UI.Services
{
    internal class BoardDisplayManager : IBoardDisplayManager
    {
        private const int LegendOffset = 5;
        private const string SymbolTemplate = " {0} ";

        private static readonly IReadOnlyDictionary<CellType, CellDisplayDetails> CellDisplayDetails = new Dictionary<CellType, CellDisplayDetails>()
        {
            { CellType.Unexplored, new CellDisplayDetails {Symbol = '~', Color = ConsoleColor.White } },
            { CellType.Hit, new CellDisplayDetails { Symbol = 'X', Color = ConsoleColor.Red, } },
            { CellType.Missed, new CellDisplayDetails { Symbol = 'O', Color = ConsoleColor.Blue } },
            { CellType.Destroyed, new CellDisplayDetails { Symbol = '■', Color = ConsoleColor.DarkRed, } }
        };

        private static readonly LegendItem[] Legend = new LegendItem[]
        {
            new LegendItem { DisplayDetails = CellDisplayDetails[CellType.Unexplored], Info = "Unexplored area" },
            new LegendItem { DisplayDetails = CellDisplayDetails[CellType.Destroyed], Info = "Destroyed ship" },
            new LegendItem { DisplayDetails = CellDisplayDetails[CellType.Missed], Info = "Hit missed" },
            new LegendItem { DisplayDetails = CellDisplayDetails[CellType.Hit], Info = "Damaged ship" },
        };


        private Coordinates _boardInitialCoordinates = new();

        private readonly IBoard _board;
        private readonly int _boardSize;

        public BoardDisplayManager(IBoard board)
        {
            _board = board;
            _boardSize = _board.GetSize();
        }

        public void InitializeBoard()
        {
            SetBoardInitialCoordinates();
            DisplayTopGridIndexes();
            DisplayGridBody();
            DisplayLegend();

            Console.WriteLine();
        }

        public void UpdateBoard()
        {
            for (var y = 0; y < _boardSize; y++)
            {
                for (var x = 0; x < _boardSize; x++)
                {
                    var coordinates = ConvertToCoordinates(x, y);
                    var cellType = _board.GetCellType(coordinates);
                    
                    DisplayCell(coordinates, cellType);
                }
            }
        }

        private static Coordinates ConvertToCoordinates(int x, int y)
            => new()
            {
                X = x,
                Y = y,
            };

        private void DisplayGridBody()
        {
            for (var y = 0; y < _boardSize; y++)
            {
                var indexLetter = (char)('A' + y);

                Console.WriteLine();
                Console.Write(SymbolTemplate, indexLetter);

                for (var x = 0; x < _boardSize; x++)
                {
                    var coordinates = ConvertToCoordinates(x, y);
                    var cellType = _board.GetCellType(coordinates);
                    var symbol = CellDisplayDetails[cellType].Symbol;

                    Console.Write(SymbolTemplate, symbol);
                }
            }
        }

        private void DisplayTopGridIndexes()
        {
            Console.Write("   ");
            for (var x = 0; x < _boardSize; x++)
            {
                Console.Write(SymbolTemplate, 1 + x);
            }
        }

        private void DisplayLegend()
        {
            var initalCursorPosition = Console.GetCursorPosition();
            Console.CursorTop = _boardInitialCoordinates.Y;

            foreach (var legendItem in Legend)
            {
                Console.CursorLeft = _boardInitialCoordinates.X * _boardSize + _boardSize + LegendOffset;
                ConsoleWriter.WriteLine($"'{legendItem.DisplayDetails.Symbol}' - {legendItem.Info}", legendItem.DisplayDetails.Color);
            }

            Console.SetCursorPosition(initalCursorPosition.Left, initalCursorPosition.Top);
        }

        private void DisplayCell(Coordinates cellCoordinates, CellType cellType)
        {
            var coordinates = new Coordinates
            {
                X = cellCoordinates.X * 3 + _boardInitialCoordinates.X,
                Y = cellCoordinates.Y + _boardInitialCoordinates.Y
            };
            var displayDetails = CellDisplayDetails[cellType];

            ConsoleWriter.Write(SymbolTemplate, displayDetails.Symbol, coordinates, displayDetails.Color);
        }

        private void SetBoardInitialCoordinates()
        {
            _boardInitialCoordinates = new()
            {
                X = Console.CursorLeft + 3,
                Y = Console.CursorTop + 1,
            };
        }
    }
}
