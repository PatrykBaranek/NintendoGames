using NintendoGames.Models;

namespace NintendoGames.Services.DataScraper
{
    public interface IDataScraper
    {
        Task<List<GameDto>> GetNintendoGames();
    }
}
