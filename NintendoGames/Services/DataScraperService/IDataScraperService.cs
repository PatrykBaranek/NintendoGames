using NintendoGames.Models.DataScraperModels;

namespace NintendoGames.Services.DataScraperService
{
    public interface IDataScraperService
    {
        Task<List<ScrapedGameFromDbDto>> GetListFromDb();
        Task<List<ScrapedGameFromDbDto>> GetGamesByNameFromDb(string gameName);
        Task<List<ScrapedGameDto>> GetNintendoGames(int startPage, int endPage, int gamesToDisplay);
        Task PostGamesToDatabase();
    }
}
