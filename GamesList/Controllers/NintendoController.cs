using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using NintendoGames.Models;
using HtmlAgilityPack.CssSelectors.NetCore;
using System.Text;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NintendoController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<GameDto>> GetAllGames()
        {
            var gamesList = GetAllGamesFromWeb();

            return Ok(gamesList);
        }

        private static List<GameDto> GetAllGamesFromWeb()
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
                .SelectNodes("//li[@data-type='game']").ToList();

                for (int j = 0; j < listOfGamesOnPage.Count; j++)
                {
                    gamesList.Add(new GameDto()
                    {
                        Name = listOfGamesOnPage[j].QuerySelector("span.title").InnerText,
                        //ImageUrl = listOfGamesOnPage[j].QuerySelector("a.img img").Attributes["src"].Value,
                        //Companies = listOfGamesOnPage[j].QuerySelector("p.description a").InnerText,
                    });
                }
            }

            return gamesList;
        }
    }
}
