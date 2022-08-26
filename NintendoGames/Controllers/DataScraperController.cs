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
        public async Task<ActionResult<List<GameDto>>> GetNintendoGamesFromWeb([FromQuery] int pages, [FromQuery] int gamesToDisplay)
        {
            var gamesFromWeb = await _dataScraperService.GetNintendoGames(pages, gamesToDisplay);

            return Ok(gamesFromWeb);
        }

        [HttpGet("list")]
        public ActionResult<List<GameDto>> GetList()
        {
            var list = _dataScraperService.GetList();

            return Ok(list);
        }

        [HttpPost("addToDatabase")]
        public ActionResult PostGameToDatabase()
        {
            _dataScraperService.PostGamesToDatabase();

            return Ok();
        }
        


    }





}


