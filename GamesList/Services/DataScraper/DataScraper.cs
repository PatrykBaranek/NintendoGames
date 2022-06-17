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


            for (int i = 0; i < 1; i++)
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
                        ReleaseDate = listOfGamesOnPage[j].SelectNodes("//td[@class = 'details']/span")[j].InnerText,
                        MoreDetails = GetMoreDatails("https://www.metacritic.com" + listOfGamesOnPage[j].SelectNodes("//div[@class = 'collapsed']/a")[j].Attributes["href"].Value),
                    }));

                }
            }
            return gamesList;
        }

        private MoreDetailsDto GetMoreDatails(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);

            MoreDetailsDto result = new MoreDetailsDto()
            {
                Developers = doc.DocumentNode.QuerySelectorAll("li.developer span.data a").Select(d => d.InnerText.Split(',')).ToList(),
                Genres = doc.DocumentNode.QuerySelectorAll(".product_genre .data").Select(ge => ge.InnerText).ToList(),
                Platforms = doc.DocumentNode.QuerySelectorAll(".product_platforms .data .hover_none").Select(p => p.InnerText).Append("Switch").ToList(),
                Ratings = new RatingDto()
                {
                    MetacriticCriticScore = doc.DocumentNode.QuerySelector("[itemprop='ratingValue']") != null ?
                        doc.DocumentNode.QuerySelector("[itemprop='ratingValue']").InnerText : string.Empty,
                    IsMustPlay = doc.DocumentNode.QuerySelector(".must_play.product") != null ? true : false
                }
            };

            return result;
        }
    }
}
