using AutoMapper;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Exceptions;
using NintendoGames.Models.DataScraper;

namespace NintendoGames.Services.DataScraper
{
    public partial class DataScraperService : IDataScraperService
    {
        private const string _URL =
            "https://www.metacritic.com/browse/games/release-date/available/switch/metascore?view=condensed";

        private readonly NintendoDbContext _dbContext;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        private static List<GameDto> _gamesList = new();

        public DataScraperService(NintendoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _client = new HttpClient();
        }

        public async Task<List<GameDto>> GetList()
        {
            if (_dbContext.Game.Any())
            {
                var gamesList = await _dbContext.Game
                    .Include(g => g.Developers)
                    .Include(g => g.Genres)
                    .Include(g => g.Rating)
                    .ToListAsync();

                var mapperList = _mapper.Map<List<Game>, List<GameDto>>(gamesList);

                _gamesList = mapperList;

                return _gamesList;
            }

            if (_gamesList.Count == 0)
            {
                throw new NoContentException("Not found games");
            }

            return _gamesList;
        }

        
        public async Task<List<GameDto>> GetNintendoGames(int startPage, int endPage, int gamesToDisplay)
        {
            var pagesOnSiteDocument = await GetHtmlDocument(_URL);

            var lastPageOnSite = int.Parse(pagesOnSiteDocument.QuerySelector("li.page.last_page").InnerText.Replace("&hellip;", ""));

            if (endPage > lastPageOnSite && startPage <= 0 || startPage > endPage && endPage < startPage)
                throw new BadRequestException("Invalid params");

            if (gamesToDisplay <= 0 || gamesToDisplay > endPage * 100)
                throw new BadRequestException("Invalid params");


            for (int i = startPage - 1; i <= endPage; i++)
            {
                var doc = await GetHtmlDocument(_URL + $"&page={i}");

                var listOfGamesOnPage = doc.DocumentNode
                .QuerySelectorAll("table.clamp-list tr.expand_collapse").ToList();

                for (int j = 0; j < listOfGamesOnPage.Count; j++)
                {
                    var game = await GetGameDetails(
                        listOfGamesOnPage[j].SelectNodes("//span[@class = 'title numbered']")[j]
                            .InnerText
                            .Replace("\n", "")
                            .Replace(".", "").Trim(),

                        listOfGamesOnPage[j].QuerySelector("a.title h3")
                            .InnerText,

                        listOfGamesOnPage[j]
                            .SelectNodes("//td[@class = 'details']/div[@class = 'collapsed']/a/img")[j]
                            .Attributes["src"].Value,

                        listOfGamesOnPage[j].SelectNodes("//td[@class = 'details']/span")[j]
                            .InnerText,

                        "https://www.metacritic.com" +
                                                   listOfGamesOnPage[j].SelectNodes("//div[@class = 'collapsed']/a")[
                                                        j].Attributes["href"].Value
                    );

                    _gamesList.Add(game);

                    if (_gamesList.Count == gamesToDisplay)
                    {
                        return _gamesList;
                    }
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
