using GamesList.Models;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using NintendoGames.Models;

namespace GamesList.Services.DataScraper
{
    public class DataScraper : IDataScraper
    {
        public async Task<List<GameDto>> GetNintendoGames()
        {
            var gamesList = new List<GameDto>();

            var url = "https://www.metacritic.com/browse/games/release-date/available/switch/metascore?view=condensed";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var pageCount = int.Parse(doc.DocumentNode
                .QuerySelector("li.last_page a.page_num").InnerText);


            for (int i = 0; i < pageCount; i++)
            {
                doc = web.Load(url + $"&page={i}");

                List<HtmlNode> listOfGamesOnPage = doc.DocumentNode
                .QuerySelectorAll("table.clamp-list tr.expand_collapse").ToList();

                for (int j = 0; j < listOfGamesOnPage.Count; j++)
                {
                    await Task.Run(() => gamesList.Add(new GameDto()
                    {
                        GameTitle = listOfGamesOnPage[j].QuerySelector("a.title h3").InnerText,
                        ImageUrl = listOfGamesOnPage[j].SelectNodes("//td[@class = 'details']/div[@class = 'collapsed']/a/img")[j].Attributes["src"].Value,
                        MoreInfoUrl = "https://www.metacritic.com" + listOfGamesOnPage[j].SelectNodes("//div[@class = 'collapsed']/a")[j].Attributes["href"].Value,
                        Ratings = new RatingDto()
                        {
                            MetacriticCriticScore = listOfGamesOnPage[j].SelectNodes("//div[@class = 'metascore_w large game positive']")[j].InnerText,
                            //MetacriticUserScore = listOfGamesOnPage[j].SelectNodes("//div[@class = 'metascore_w user large game positive']")[j].InnerText,
                            //IsMustPlay = listOfGamesOnPage[j].QuerySelector(".mcmust") != null ? true : false,
                        }
                    }));

                }
            }
            return gamesList;
        }

        public List<PriceDto> GetPrices()
        {
            throw new NotImplementedException();
        }


    }
}
