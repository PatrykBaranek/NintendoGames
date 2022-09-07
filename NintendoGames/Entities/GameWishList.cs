namespace NintendoGames.Entities
{
    public class GameWishList
    {
        public Guid GameId { get; set; }
        public Game Game { get; set; }

        public Guid WishListId { get; set; }
        public WishList WishList { get; set; }
    }
}
