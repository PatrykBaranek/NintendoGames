using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.RatingModels;
using NintendoGames.Services.RatingService;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Route("api/games/{gameId:guid}/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPatch("update")]
        public async Task<ActionResult> UpdateRating([FromRoute] Guid gameId, [FromBody] UpdateUserScoreDto updateUserScore)
        {
            await _ratingService.UpdateUserScore(gameId, updateUserScore);

            return Ok();
        }
    }
}
