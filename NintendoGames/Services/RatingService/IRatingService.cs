using NintendoGames.Models.RatingModels;

namespace NintendoGames.Services.RatingService
{
    public interface IRatingService
    {
        Task UpdateUserScore(Guid gameId, UpdateUserScoreDto updateUserScoreDto);
    }
}
