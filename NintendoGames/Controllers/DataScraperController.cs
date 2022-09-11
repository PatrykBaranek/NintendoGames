using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.DataScraperModels;
using NintendoGames.Services.DataScraperService;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
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

        [HttpGet("showAllRelatedGamesFromDatabase")]
        public async Task<ActionResult<List<ScrapedGameDto>>> GetGamesByNameFromDb([FromQuery] string gameName)
        {
            var listOfRelatedGames = await _dataScraperService.GetGamesByNameFromDb(gameName);

            return Ok(listOfRelatedGames);
        }

        [HttpGet("showAllGamesFromDatabase")]
        public async Task<ActionResult<List<ScrapedGameDto>>> GetListFromDb()
        {
            var list = await _dataScraperService.GetListFromDb();

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


