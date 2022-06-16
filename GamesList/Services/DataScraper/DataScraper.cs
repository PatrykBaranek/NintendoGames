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

                var listOfGamesOnPage = doc.DocumentNode
                .QuerySelectorAll("table.clamp-list").ToList();

                for (int j = 0; j < listOfGamesOnPage.Count; j++)
                {
                    await Task.Run(() => gamesList.Add(new GameDto()
                    {
                        GameTitle = listOfGamesOnPage[j].QuerySelector("a.title h3").InnerText,
                        ImageUrl = listOfGamesOnPage[j].SelectSingleNode("//div[@class = 'collapsed']/a/img").Attributes["src"].Value,
                        MoreInfoUrl = "https://www.metacritic.com" + listOfGamesOnPage[j].SelectSingleNode("//div[@class = 'collapsed']/a").Attributes["href"].Value,
                        Ratings = new RatingDto()
                        {
                            //MetacriticCriticScore = listOfGamesOnPage[j].QuerySelector(".metascore_w.large.game.positive").InnerText,
                            //MetacriticUserScore = listOfGamesOnPage[j].QuerySelector(".collapsed a.metascore_anchor .metascore_w.user.large.game.positive").InnerText,
                            //IsMustPlay = listOfGamesOnPage[j].QuerySelector(".mcmust") != null,
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
