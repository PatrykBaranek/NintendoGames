using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using NintendoGames.Models;

namespace NintendoGames.Services.DataScraper
{
    public class DataScraperService : IDataScraperService
    {
        private const string URL =
            "https://www.metacritic.com/browse/games/release-date/available/switch/metascore?view=condensed";

        public async Task<List<GameDto>> GetNintendoGames()
        {
            var gamesList = new List<GameDto>();

            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(URL);

            var pageCount = int.Parse(doc.DocumentNode
                .QuerySelector("li.last_page a.page_num").InnerText);


            for (int i = 0; i < 1; i++)
            {
                doc = web.Load(URL + $"&page={i}");

                var listOfGamesOnPage = doc.DocumentNode
                .QuerySelectorAll("table.clamp-list tr.expand_collapse").ToList();

                for (int j = 0; j < listOfGamesOnPage.Count; j++)
                {
                    await Task.Run(() => gamesList.Add(new GameDto
                    {
                        Id = listOfGamesOnPage[j].SelectNodes("//span[@class = 'title numbered']")[j].InnerText,
                        
                        GameTitle = listOfGamesOnPage[j].QuerySelector("a.title h3").InnerText,
                        
                        ImageUrl = listOfGamesOnPage[j]
                            .SelectNodes("//td[@class = 'details']/div[@class = 'collapsed']/a/img")[j]
                            .Attributes["src"].Value,

                        ReleaseDate = listOfGamesOnPage[j].SelectNodes("//td[@class = 'details']/span")[j].InnerText,

                        MoreDetails = GetMoreDatails("https://www.metacritic.com" +
                                                     listOfGamesOnPage[j].SelectNodes("//div[@class = 'collapsed']/a")[
                                                         j].Attributes["href"].Value),
                    }));
                }
            }
            return gamesList;
        }

        private MoreDetailsDto GetMoreDatails(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var result = new MoreDetailsDto
            {
                Developers = doc.DocumentNode.QuerySelectorAll("li.developer span.data a").Select(d => d.InnerText.Split(',')).ToList(),
                Genres = doc.DocumentNode.QuerySelectorAll(".product_genre .data").Select(ge => ge.InnerText).ToList(),
                Ratings = new RatingDto
                {
                    CriticRating = doc.DocumentNode.QuerySelector("[itemprop='ratingValue']") != null ?
                       doc.DocumentNode.QuerySelector("[itemprop='ratingValue']").InnerText : string.Empty,
                    UserScore = doc.DocumentNode.QuerySelector("#main > div > div:nth-child(1) > div.left > div.module.product_data.product_data_summary > div > div.summary_wrap > div.section.product_scores > div.details.side_details > div:nth-child(1) > div > a > div") != null ?
                       doc.DocumentNode.QuerySelector("#main > div > div:nth-child(1) > div.left > div.module.product_data.product_data_summary > div > div.summary_wrap > div.section.product_scores > div.details.side_details > div:nth-child(1) > div > a > div").InnerText : string.Empty,
                    IsMustPlay = doc.DocumentNode.QuerySelector(".must_play.product") != null
                }
            };

            return result;
        }
    }
}
