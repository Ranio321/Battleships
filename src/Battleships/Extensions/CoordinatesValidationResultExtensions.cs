using Battleships.Boards.Models;
using Battleships.Models;

namespace Battleships.Extensions
{
    internal static class CoordinatesValidationResultExtensions
    {
        public static ShootResult ToShootResult(this CoordinatesValidationError error)
            => error switch
            {
                CoordinatesValidationError.AlreadyUsed => ShootResult.CoordinatesAlreadyUsed,
                CoordinatesValidationError.OutOfBounds => ShootResult.CoordinatesOutOfBounds,
                _ => throw new NotImplementedException(),
            };
    }
}
