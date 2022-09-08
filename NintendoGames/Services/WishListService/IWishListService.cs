using NintendoGames.Models.WishListModels;

namespace NintendoGames.Services.WishListService
{
    public interface IWishListService
    {
        Task<List<WishListDto>> ShowAllGamesUserWishList();
        Task AddGameToWishList(AddGameToWishListDto addGameToWishListDto);
        Task DeleteGameFromWishList(DeleteGameFromWishListDto deleteGameFromWishListDto);
    }
}
