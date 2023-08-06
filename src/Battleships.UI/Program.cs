using Battleships.Boards;
using Battleships.Services;
using Battleships.Ships;
using Battleships.UI;
using Battleships.UI.Services;

namespace Battleships
{
    public class Program
    {
        private static readonly int BoardSize = Configuration.BoardSize;
        private static readonly List<Ship> Ships = Configuration.GetShips();

        static void Main()
        {
            var gameManager = CreateGameManager();

            gameManager.RandomizeShipsPlacement(Ships);
            gameManager.Play();
        }

        private static ConsoleGameManager CreateGameManager()
        {
            var board = new Board(BoardSize);
            var coordinatesParser = new CoordinatesParser(BoardSize);
            var shipPlacementRandomizer = new ShipPlacementRandomizer();
            var coordinatesValidator = new CoordinatesValidator(board);
            var battleshipsGame = new BattleshipsGame(board, shipPlacementRandomizer, coordinatesValidator);
            var boardDisplayManager = new BoardDisplayManager(board);

            var gameManager = new ConsoleGameManager(battleshipsGame, boardDisplayManager, coordinatesParser);

            return gameManager;
        }
    }
}