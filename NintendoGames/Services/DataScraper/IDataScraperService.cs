using NintendoGames.Models.DataScraper;

namespace NintendoGames.Services.DataScraper
{
    public interface IDataScraperService
    {
        Task<List<ScrapedGameDto>> GetList();
        Task<List<ScrapedGameDto>> GetNintendoGames(int startPage, int endPage, int gamesToDisplay);
        Task PostGamesToDatabase();
    }
}
