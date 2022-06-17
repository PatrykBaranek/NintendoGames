using GamesList.Entities;
using GamesList.Services.DataScraper;
using NintendoGames.Models;

namespace GamesList.Services
{
    public class NintendoService : INintendoService
    {
        private readonly IDataScraper _dataScraper;
        private readonly NintendoDbContext _dbContext;
        private static List<GameDto> _games;


        public NintendoService(NintendoDbContext dbContext, IDataScraper dataScraper)
        {
            _dbContext = dbContext;
            _dataScraper = dataScraper;
        }

        public async Task<List<GameDto>> GetAllGamesFromWeb()
        {
            //_games = await _dataScraper.GetNintendoGames();

            return await _dataScraper.GetNintendoGames();
        }

        
    }
}
