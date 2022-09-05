using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.GamesModels;
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

        [HttpGet("game")]
        public async Task<ActionResult<List<GameDto>>> GetGamesByName([FromQuery] string gameName)
        {
            var relatedGames = await _gamesService.GetGamesByName(gameName);

            return Ok(relatedGames);
        }

        [HttpDelete("delete/{gameId:guid}")]
        public async Task<ActionResult> DeleteGame([FromRoute] Guid gameId)
        {
            await _gamesService.DeleteGame(gameId);

            return Ok();
        }
    }
}
