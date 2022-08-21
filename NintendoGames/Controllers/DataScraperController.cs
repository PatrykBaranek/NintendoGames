using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models;
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
        public async Task<ActionResult<List<GameDto>>> GetNintendoGamesFromWeb()
        {
            var gamesFromWeb = await _dataScraperService.GetNintendoGames();

            return Ok(gamesFromWeb);
        }

        
    }





}


