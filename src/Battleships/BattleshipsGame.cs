using Battleships.Boards.Models;
using Battleships.Ships;
using Battleships.Boards;
using Battleships.Models;
using Battleships.Exceptions;
using Battleships.Extensions;
using Battleships.Services.Abstraction;

namespace Battleships
{
    public class BattleshipsGame : IBattleshipsGame
    {
        public GameState GameState { get; private set; } = GameState.PlacementPhase;

        private readonly List<Ship> _ships = new();

        private readonly IBoard _board;
        private readonly IShipPlacementService _shipPlacementService;
        private readonly ICoordinatesValidator _coordinatesValidator;

        public BattleshipsGame(
            IBoard board,
            IShipPlacementService shipPlacementService,
            ICoordinatesValidator coordinatesValidator)
        {
            _board = board;
            _shipPlacementService = shipPlacementService;
            _coordinatesValidator = coordinatesValidator;
        }

        public void PlaceShips(IEnumerable<Ship> ships)
        {
            CheckGameState(GameState.PlacementPhase);

            _ships.AddRange(ships); 
            _shipPlacementService.PlaceShips(_board, _ships);

            GameState = GameState.InProgress;
        }

        public CoordinatesValidationError? ValidateCoordinates(Coordinates coordinates)
            => _coordinatesValidator.ValidateCoordinates(coordinates);

        public void Shoot(Coordinates coordinates)
        {
            CheckGameState(GameState.InProgress);

            var validationResult = _coordinatesValidator.ValidateCoordinates(coordinates);

            if (validationResult is not null)
                throw new InvalidCoordinatesException(coordinates);

            var affectedShip = _ships
                .Where(x => x.GetShipLocation()
                    .Any(x => x.X == coordinates.X && x.Y == coordinates.Y))
                .FirstOrDefault();

            if (affectedShip is null)
            {
                _board.SetCellType(coordinates, CellType.Missed);
            }
            else
            {
                affectedShip.ReceiveHit();

                if (affectedShip.Destroyed)
                {
                    foreach (var shipCoordinates in affectedShip.GetShipLocation())
                    {
                        _board.SetCellType(shipCoordinates, CellType.Destroyed);
                    }
                }
                else
                {
                    _board.SetCellType(coordinates, CellType.Hit);
                }
            }

            CheckIfAllShipsDestroyed();
        }

        private void CheckGameState(GameState desirableState)
        {
            if (desirableState != GameState)
                throw new InvalidGameStateException($"Invalid game state {GameState}");
        }

        private void CheckIfAllShipsDestroyed()
        {
            var allShipsDestroyed = _ships.All(x => x.Destroyed);
            if (allShipsDestroyed)
            {
                GameState = GameState.Finished;
            }
        }
    }
}
