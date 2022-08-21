using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using NintendoGames.Entities;
using NintendoGames.Models.DataScraper;

namespace NintendoGames.Services.DataScraper
{
    public class DataScraperService : IDataScraperService
    {
        private const string URL =
            "https://www.metacritic.com/browse/games/release-date/available/switch/metascore?view=condensed";


        private readonly NintendoDbContext _dbContext;
        private readonly HttpClient _client;
        private readonly List<GameDto> _gamesList;

        public DataScraperService(NintendoDbContext dbContext)
        {
            _dbContext = dbContext;
            _client = new HttpClient();
            _gamesList = new List<GameDto>();
        }

        public List<GameDto> GetList()
        {
            if (_gamesList.Count == 0)
            {
                return null;
            }

            return _gamesList;
        }


        public async Task<List<GameDto>> GetNintendoGames()
        {
            for (int i = 0; i <= 2; i++)
            {
                var doc = await GetHtmlDocument(URL + $"&page={i}");

                var listOfGamesOnPage = doc.DocumentNode
                .QuerySelectorAll("table.clamp-list tr.expand_collapse").ToList();

                for (int j = 0; j < listOfGamesOnPage.Count; j++)
                {
                    var game = await GetGameDetails(
                        listOfGamesOnPage[j].SelectNodes("//span[@class = 'title numbered']")[j].InnerText,

                        listOfGamesOnPage[j].QuerySelector("a.title h3").InnerText,

                        listOfGamesOnPage[j]
                            .SelectNodes("//td[@class = 'details']/div[@class = 'collapsed']/a/img")[j]
                            .Attributes["src"].Value,

                        listOfGamesOnPage[j].SelectNodes("//td[@class = 'details']/span")[j].InnerText,

                        "https://www.metacritic.com" +
                                                   listOfGamesOnPage[j].SelectNodes("//div[@class = 'collapsed']/a")[
                                                        j].Attributes["href"].Value
                    );

                    _gamesList.Add(game);
                }
            }

            return _gamesList;
        }

        private async Task<GameDto> GetGameDetails(string id, string gameTitle, string imgUrl, string releaseDate, string moreDetailsUrl)
        {
            var moreDetails = await GetMoreDetails(moreDetailsUrl);

            var gameDto = new GameDto
            {
                Id = id,
                GameTitle = gameTitle,
                ImageUrl = imgUrl,
                ReleaseDate = releaseDate,
                MoreDetails = moreDetails,
            };

            return gameDto;
        }


        private async Task<MoreDetailsDto> GetMoreDetails(string url)
        {

            var doc = await GetHtmlDocument(url);

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

        private async Task<HtmlDocument> GetHtmlDocument(string url)
        {
            var html = await _client.GetStringAsync(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            return doc;
        }
    }
}
