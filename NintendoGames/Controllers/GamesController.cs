using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.Games;
using NintendoGames.Services.Games;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGamesService _gamesService;
        public GamesController(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameDto>>> GetAllGames()
        {
            var gamesList = await _gamesService.GetAllGames();

            return Ok(gamesList);
        }

        [HttpGet("{gameName}")]
        public async Task<ActionResult<List<GameDto>>> GetGame([FromRoute] string gameName)
        {
            var relatedGames = await _gamesService.GetGame(gameName);

            return Ok(relatedGames);
        }
    }
}
