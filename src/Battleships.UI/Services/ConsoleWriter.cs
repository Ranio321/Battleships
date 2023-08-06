using Battleships.Models;

namespace Battleships.UI.Services
{
    public static class ConsoleWriter
    {
        public static void WriteEmptyLine()
            => Console.WriteLine();

        public static void WriteLine(string message)
            => WriteLine(message, Console.ForegroundColor);

        public static void WriteLine(string message, ConsoleColor color)
        {
            var initialColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = initialColor;
        }

        public static void Write(string message, object? arg0, Coordinates coordinates, ConsoleColor color)
        {
            var initialCursorPosition = Console.GetCursorPosition();
            var initalForegroundColor = Console.ForegroundColor;

            Console.SetCursorPosition(coordinates.X, coordinates.Y);
            Console.ForegroundColor = color;
            Console.Write(message, arg0);

            Console.SetCursorPosition(initialCursorPosition.Left, initialCursorPosition.Top);
            Console.ForegroundColor = initalForegroundColor;
        }
    }
}
