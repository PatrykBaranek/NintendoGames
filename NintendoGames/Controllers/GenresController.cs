using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.DevelopersModels;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Route("api/games/{gameId:guid}/[controller]")]
    public class GenresController : ControllerBase
    {


        [HttpPost("add")]
        public async Task<ActionResult> AddGenre([FromRoute] Guid gameId, [FromBody] AddDeveloperDto developerDto)
        {
            await _developersService.AddDeveloper(gameId, developerDto);

            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteGenre([FromRoute] Guid gameId,
            [FromBody] DeleteDeveloperDto deleteDeveloperDto)
        {
            await _developersService.DeleteDeveloper(gameId, deleteDeveloperDto);

            return Ok();
        }
    }
}
