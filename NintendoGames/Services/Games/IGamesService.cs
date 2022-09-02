using NintendoGames.Models;
using NintendoGames.Models.Games;

namespace NintendoGames.Services.Games
{
    public interface IGamesService
    {
        Task AddGame();
        Task<List<GameDto>> GetAllGames();
        Task<List<GameDto>> GetGame(string gameName);
        Task UpdateGame();
        Task DeleteGame();

    }
}