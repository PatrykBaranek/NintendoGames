using NintendoGames.Models;

namespace NintendoGames.Services.DataScraper
{
    public interface IDataScraperService
    {
        Task<List<GameDto>> GetNintendoGames();
    }
}
