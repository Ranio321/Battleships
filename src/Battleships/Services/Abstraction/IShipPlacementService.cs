using Battleships.Ships;
using Battleships.Boards;

namespace Battleships.Services.Abstraction
{
    public interface IShipPlacementService
    {
        void PlaceShips(IBoard grid, IEnumerable<Ship> ships);
    }
}
