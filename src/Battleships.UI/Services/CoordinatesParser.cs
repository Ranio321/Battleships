using Battleships.Models;
using Battleships.UI.Services.Abstraction;

namespace Battleships.UI.Services
{
    internal class CoordinatesParser : ICoordinatesParser
    {
        private readonly int _boardSize;

        public CoordinatesParser(int boardSize)
        {
            _boardSize = boardSize;
        }

        public Coordinates? ConvertToCoordinates(string? input)
        {
            if (input is null || input.Length < 2)
            {
                return null;
            }

            var yValue = input[0];

            if (yValue < 'A' || yValue > 'A' + _boardSize)
            {
                return null;
            }

            var xStringValue = new string(input.Skip(1).ToArray());

            if (!int.TryParse(xStringValue, out var xValue))
            {
                return null;
            }

            if (xValue < 1 || xValue > 1 + _boardSize)
            {
                return null;
            }

            return new()
            {
                X = xValue - 1,
                Y = yValue - 'A',
            };
        }
    }
}
