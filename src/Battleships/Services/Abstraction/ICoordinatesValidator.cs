using Battleships.Boards.Models;
using Battleships.Models;

namespace Battleships.Services.Abstraction
{
    public interface ICoordinatesValidator
    {
        CoordinatesValidationError? ValidateCoordinates(Coordinates coordinates);
    }
}
