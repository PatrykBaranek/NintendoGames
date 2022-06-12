using GamesList.Services.DataScraper;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using NintendoGames.Models;

namespace GamesList.Services
{
    public class NintendoService : INintendoService
    {
        private readonly IDataScraper _dataScraper;

        public NintendoService(IDataScraper dataScraper)
        {
            _dataScraper = dataScraper;
        }

        public async Task<List<GameDto>> GetAllGamesFromWeb()
        {
            return await _dataScraper.GetNintendoGames();
        }

        
    }
}
