using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.DevelopersModels;
using NintendoGames.Services.DevelopersService;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Route("api/games/{gameId:guid}/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DevelopersController : ControllerBase
    {
        private readonly IDevelopersService _developersService;

        public DevelopersController(IDevelopersService developersService)
        {
            _developersService = developersService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DevelopersDto>>> GetAllGameDevelopers([FromRoute] Guid gameId)
        {
            var developers = await _developersService.GetAllGameDevelopers(gameId);

            return Ok(developers);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddDeveloper([FromRoute] Guid gameId, [FromBody] AddDeveloperDto developerDto)
        {
            await _developersService.AddDeveloper(gameId, developerDto);

            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteDeveloper([FromRoute] Guid gameId,
            [FromBody] DeleteDeveloperDto deleteDeveloperDto)
        {
            await _developersService.DeleteDeveloper(gameId, deleteDeveloperDto);

            return Ok();
        }
    }
}
