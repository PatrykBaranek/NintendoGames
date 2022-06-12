using GamesList.Models;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using NintendoGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesList.Services.DataScraper
{
    public class DataScraper : IDataScraper
    {
        public async Task<List<GameDto>> GetNintendoGames()
        {
            var gamesList = new List<GameDto>();

            var url = "https://www.nintendolife.com/games/browse?sort=popular";
            var web = new HtmlWeb();
            var doc = web.Load(url);


            var pageCount = doc.DocumentNode
                .QuerySelectorAll("ul.paginate li")
                .Skip(1)
                .ToList();


            for (int i = 0; i < pageCount.Count; i++)
            {
                doc = web.Load(url + $"&status=released&page={i + 1}&system=nintendo-switch");

                var listOfGamesOnPage = doc.DocumentNode
                .QuerySelectorAll("[data-type='game']").ToList();

                for (int j = 0; j < listOfGamesOnPage.Count; j++)
                {
                    await Task.Run(() => gamesList.Add(new GameDto()
                    {
                        Name = listOfGamesOnPage[j].QuerySelector("span.title").InnerText,
                        ImageUrl = listOfGamesOnPage[j].SelectSingleNode("//div[@class = 'cover']/a[@class = 'img']/img").Attributes["src"].Value,
                        Companies = GetAllCompanies(listOfGamesOnPage[j].QuerySelectorAll("p.description a").ToList()),
                    }));
                }
            }

            return gamesList;
        }

        public List<PriceDto> GetPrices()
        {
            throw new NotImplementedException();
        }

        public List<RatingDto> GetRatings()
        {
            throw new NotImplementedException();
        }

        private List<string> GetAllCompanies(List<HtmlNode> listOfNodes)
        {
            List<string> companies = new List<string>();

            foreach (var item in listOfNodes)
            {
                companies.Add(item.InnerText);
            }

            return companies;
        }
    }
}
