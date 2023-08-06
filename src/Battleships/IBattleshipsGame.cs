using Battleships.Ships;
using Battleships.Models;
using Battleships.Boards.Models;

namespace Battleships
{
    public interface IBattleshipsGame
    {
        GameState GameState { get; }
        void PlaceShips(IEnumerable<Ship> ships);
        CoordinatesValidationError? ValidateCoordinates(Coordinates coordinates);
        void Shoot(Coordinates coordinates);
    }
}
