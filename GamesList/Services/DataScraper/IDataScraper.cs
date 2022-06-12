using GamesList.Models;
using NintendoGames.Models;

namespace GamesList.Services.DataScraper
{
    public interface IDataScraper
    {
        Task<List<GameDto>> GetNintendoGames();
    }
}
