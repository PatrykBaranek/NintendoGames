namespace NintendoGames.Entities
{
    public class WishList
    {
        public Guid Id { get; set; }


        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<GameWishList> GameWishLists { get; set; }
    }
}
