using NintendoGames.Models;

namespace NintendoGames.Services
{
    public interface INintendoService
    {
        Task<List<GameDto>> GetAllGamesFromWeb();
    }
}