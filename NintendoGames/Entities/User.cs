namespace NintendoGames.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Guid WishListId { get; set; }
        public WishList WishList { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
