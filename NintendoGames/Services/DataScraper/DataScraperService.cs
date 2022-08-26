using AutoMapper;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using NintendoGames.Entities;
using NintendoGames.Exceptions;
using NintendoGames.Models.DataScraper;

namespace NintendoGames.Services.DataScraper
{
    public class DataScraperService : IDataScraperService
    {
        private const string _URL =
            "https://www.metacritic.com/browse/games/release-date/available/switch/metascore?view=condensed";

        private readonly NintendoDbContext _dbContext;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        private static readonly List<GameDto> GamesList = new();

        public DataScraperService(NintendoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _client = new HttpClient();
        }

        public List<GameDto> GetList()
        {
            if (GamesList.Count == 0)
            {
                throw new NoContentException("Not found games");
            }

            return GamesList;
        }

        public async Task PostGamesToDatabase()
        {
            if (GamesList.Count == 0)
            {
                throw new NoContentException("Not found games");
            }

            foreach (var gameDto in GamesList)
            {
                var gameToDb = new Game
                {
                    Id = Guid.NewGuid(),
                    Title = gameDto.GameTitle,
                    ImageUrl = gameDto.ImageUrl,
                    ReleaseDate = FormatReleaseDate(gameDto.ReleaseDate)
                };

                var ratingToGame = new Rating
                {
                    Id = Guid.NewGuid(),
                    CriticRating = int.Parse(gameDto.MoreDetails.Ratings.CriticRating),
                    UserScore = double.Parse(gameDto.MoreDetails.Ratings.UserScore),
                    IsMustPlay = gameDto.MoreDetails.Ratings.IsMustPlay,
                    GameId = gameToDb.Id
                };

                gameToDb.RatingId = ratingToGame.Id;

                await _dbContext.Game.AddAsync(gameToDb);

                foreach (var developers in gameDto.MoreDetails.Developers)
                {
                    foreach (var developer in developers)
                    {
                        var developerToGame = new Developers
                        {
                            Id = Guid.NewGuid(),
                            Name = developer,
                            GameId = gameToDb.Id
                        };

                        await _dbContext.Developers.AddAsync(developerToGame);
                    }
                }

                foreach (var genre in gameDto.MoreDetails.Genres)
                {
                    var genresToGame = new Genres
                    {
                        Id = Guid.NewGuid(),
                        Name = genre,
                        GameId = gameToDb.Id
                    };

                    await _dbContext.Genres.AddAsync(genresToGame);
                }

                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<List<GameDto>> GetNintendoGames(int pages, int gamesToDisplay)
        {
            var pagesOnSiteDocument = await GetHtmlDocument(_URL);

            var lastPageOnSite = int.Parse(pagesOnSiteDocument.QuerySelector("li.page.last_page").InnerText.Replace("&hellip;", ""));

            if (pages > lastPageOnSite && pages <= 0)
                throw new BadRequestException("Invalid");

            if (gamesToDisplay <= 0)
                throw new BadRequestException("Invalid");


            for (int i = 0; i <= pages; i++)
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

                    GamesList.Add(game);

                    if (GamesList.Count == gamesToDisplay)
                    {
                        return GamesList;
                    }
                }
            }

            return GamesList;
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

        private DateTime FormatReleaseDate(string dateToFormat)
        {
            var dateAsArrayString = dateToFormat.Split(' ');

            var month = dateAsArrayString[0];
            var day = dateAsArrayString[1].Replace(",", "");
            var year = dateAsArrayString[2];

            return DateTime.Parse(string.Join("/", month, day, year));

        }
    }
}
