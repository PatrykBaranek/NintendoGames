using NintendoGames.Models.DataScraper;

namespace NintendoGames.Services.DataScraper
{
    public interface IDataScraperService
    {
        Task<List<GameDto>> GetList();
        Task<List<GameDto>> GetNintendoGames(int startPage, int endPage, int gamesToDisplay);
        Task PostGamesToDatabase();
    }
}
