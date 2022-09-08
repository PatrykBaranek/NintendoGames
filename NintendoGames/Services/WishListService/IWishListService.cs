using NintendoGames.Models.GamesModels;
using NintendoGames.Models.WishListModels;

namespace NintendoGames.Services.WishListService
{
    public interface IWishListService
    {
        Task<List<GameDto>> ShowAllGamesUserWishList();
        Task AddGameToWishList(AddGameToWishListDto addGameToWishListDto);
    }
}
