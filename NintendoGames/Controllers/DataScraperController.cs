using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.DataScraper;
using NintendoGames.Services.DataScraper;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataScraperController : ControllerBase
    {

        private readonly IDataScraperService _dataScraperService;

        public DataScraperController(IDataScraperService dataScraperService)
        {
            _dataScraperService = dataScraperService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ScrapedGameDto>>> GetNintendoGamesFromWeb([FromQuery] int startPage, [FromQuery] int endPage, [FromQuery] int gamesToDisplay)
        {
            var gamesFromWeb = await _dataScraperService.GetNintendoGames(startPage, endPage, gamesToDisplay);

            return Ok(gamesFromWeb);
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<ScrapedGameDto>>> GetList()
        {
            var list = await _dataScraperService.GetList();

            return Ok(list);
        }

        [HttpPost("addToDatabase")]
        public async Task<ActionResult> PostGameToDatabase()
        {
            await _dataScraperService.PostGamesToDatabase();

            return Ok();
        }



    }





}


