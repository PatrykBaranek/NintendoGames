﻿using AutoMapper;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using NintendoGames.Entities;
using NintendoGames.Exceptions;
using NintendoGames.Models.DataScraperModels;

namespace NintendoGames.Services.DataScraperService
{
    public partial class DataScraperService : IDataScraperService
    {
        private const string Url =
            "https://www.metacritic.com/browse/games/release-date/available/switch/metascore?view=condensed";

        private readonly HttpClient _client;
        private readonly NintendoDbContext _dbContext;
        private static readonly List<ScrapedGameDto> GamesList = new();

        public DataScraperService(NintendoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _client = new HttpClient();
        }

        public async Task<List<ScrapedGameDto>> GetNintendoGames(int startPage, int endPage, int gamesToDisplay)
        {
            if (GamesList.Any()) GamesList.Clear();

            var pagesOnSiteDocument = await GetHtmlDocument(Url);

            var lastPageOnSite = int.Parse(pagesOnSiteDocument.QuerySelector("li.page.last_page").InnerText.Replace("&hellip;", ""));

            if (endPage > lastPageOnSite || startPage <= 0 || startPage > endPage && endPage < startPage)
                throw new BadRequestException("Invalid params");

            if (gamesToDisplay <= 0 || gamesToDisplay > ((endPage + 1) - startPage) * 100)
                throw new BadRequestException("Invalid params");


            for (int i = startPage - 1; i <= endPage - 1; i++)
            {
                var doc = await GetHtmlDocument(Url + $"&page={i}");

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

        private async Task<ScrapedGameDto> GetGameDetails(string id, string gameTitle, string imgUrl, string releaseDate, string moreDetailsUrl)
        {
            var moreDetails = await GetMoreDetails(moreDetailsUrl);

            var gameDto = new ScrapedGameDto
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
