using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.GenresModels;
using NintendoGames.Services.GenresService;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/games/{gameId:guid}/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;

        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenresDto>>> GetAllGameGenres([FromRoute] Guid gameId)
        {
            var gameGenres = await _genresService.GetAllGameGenres(gameId);

            return Ok(gameGenres);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddGenre([FromRoute] Guid gameId, [FromBody] AddGenreDto addGenreDto)
        {
            await _genresService.AddGenre(gameId, addGenreDto);

            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteGenre([FromRoute] Guid gameId,
            [FromBody] DeleteGenreDto deleteGenreDto)
        {
            await _genresService.DeleteGenre(gameId, deleteGenreDto);

            return Ok();
        }
    }
}
