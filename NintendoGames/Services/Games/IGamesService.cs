using NintendoGames.Models.Games;

namespace NintendoGames.Services.Games
{
    public interface IGamesService
    {
        Task<List<GameDto>> GetAllGames();
        Task<List<GameDto>> GetGamesByQuery(string gameName);
        Task DeleteGame(Guid gameId);

    }
}