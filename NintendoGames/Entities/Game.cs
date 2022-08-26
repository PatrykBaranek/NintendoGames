namespace NintendoGames.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ImageUrl { get; set; }


        public Guid RatingId { get; set; }
        public Rating Rating { get; set; }
        public ICollection<Developers> Developers { get; set; }
        public ICollection<Genres> Genres { get; set; }
    }
}
