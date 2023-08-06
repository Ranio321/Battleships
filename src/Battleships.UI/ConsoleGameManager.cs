using Battleships.Models;
using Battleships.Ships;
using Battleships.UI.Models;
using Battleships.UI.Services;
using Battleships.UI.Services.Abstraction;

namespace Battleships
{
    internal class ConsoleGameManager
    {
        private CursorPosition _initialCursorPosition = new();

        private readonly IBattleshipsGame _battleshipsGame;
        private readonly IBoardDisplayManager _boardDisplayManager;
        private readonly ICoordinatesParser _coordinatesParser;

        public ConsoleGameManager(
            IBattleshipsGame battleships,
            IBoardDisplayManager boardDisplayManager,
            ICoordinatesParser coordinatesParser)
        {
            _battleshipsGame = battleships;
            _boardDisplayManager = boardDisplayManager;
            _coordinatesParser = coordinatesParser;
        }

        public void RandomizeShipsPlacement(IEnumerable<Ship> ships)
            => _battleshipsGame.PlaceShips(ships);

        public void Play()
        {
            _boardDisplayManager.InitializeBoard();

            ConsoleWriter.WriteEmptyLine();
            SetInitialCursorPosition();

            while (_battleshipsGame.GameState == GameState.InProgress)
            {
                var coordinates = GetCoordinates();

                _battleshipsGame.Shoot(coordinates);

                _boardDisplayManager.UpdateBoard();
            }

            DisplayGameFinished();
        }

        private Coordinates GetCoordinates()
        {
            ConsoleWriter.WriteLine("Choose coordinates: ");

            bool validCoordinates;
            Coordinates? coordinates;
            do
            {
                var userInput = GetUserInput();
                coordinates = _coordinatesParser.ConvertToCoordinates(userInput);
                validCoordinates = ValidateCoordinates(coordinates);
                if (!validCoordinates)
                {
                    ConsoleWriter.WriteLine("Invalid coordinates. Please choose different one.");
                }
            }

            while (!validCoordinates);

            return coordinates!;
        }

        private bool ValidateCoordinates(Coordinates? coordinates)
            => coordinates != null && _battleshipsGame.ValidateCoordinates(coordinates) == null;

        private string GetUserInput()
        {
            string? userInput = null;

            while (userInput == null)
            {
                userInput = Console.ReadLine();
            }

            ClearConsolePrompt();

            return userInput;
        }

        private void ClearConsolePrompt()
        {
            Console.SetCursorPosition(_initialCursorPosition.Left, _initialCursorPosition.Top);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(_initialCursorPosition.Left, _initialCursorPosition.Top + 1);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(_initialCursorPosition.Left, _initialCursorPosition.Top);
        }

        private void SetInitialCursorPosition()
            => _initialCursorPosition = new()
            {
                Left = Console.CursorLeft,
                Top = Console.CursorTop,
            };

        private static void DisplayGameFinished()
            => ConsoleWriter.WriteLine("You have won", ConsoleColor.Green);
    }
}
