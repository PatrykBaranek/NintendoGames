using HtmlAgilityPack;
using NintendoGames.Models;

namespace GamesList.Services
{
    public interface INintendoService
    {
        Task<List<GameDto>> GetAllGamesFromWeb();
    }
}