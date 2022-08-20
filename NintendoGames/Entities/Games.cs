namespace NintendoGames.Entities
{
    public class Games
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ImageUrl { get; set; }

        public Guid RatingId { get; set; }
        public Ratings Ratings { get; set; }
    }
}
