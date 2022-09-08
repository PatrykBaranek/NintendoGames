using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.WishListModels;
using NintendoGames.Services.WishListService;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        [HttpGet("showAllGames")]
        public async Task<ActionResult<List<WishListDto>>> ShowAllGamesUserWishList()
        {
            var list = await _wishListService.ShowAllGamesUserWishList();

            return Ok(list);
        }

        [HttpPost("addGame")]
        public async Task<ActionResult> AddGameToWishList([FromBody] AddGameToWishListDto addGameToWishListDto)
        {
            await _wishListService.AddGameToWishList(addGameToWishListDto);

            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteGameFromWishList(
            [FromBody] DeleteGameFromWishListDto deleteGameFromWishListDto)
        {
            await _wishListService.DeleteGameFromWishList(deleteGameFromWishListDto);

            return Ok();
        }


    }
}
