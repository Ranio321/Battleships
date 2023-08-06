using Battleships.Models;

namespace Battleships.UI.Services.Abstraction
{
    internal interface ICoordinatesParser
    {
        Coordinates? ConvertToCoordinates(string? input);
    }
}
