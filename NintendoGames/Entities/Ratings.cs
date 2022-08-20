namespace NintendoGames.Entities
{
    public class Ratings
    {
        public Guid Id { get; set; }
        public int MetacriticCriticRating { get; set; }

        public Guid GameId { get; set; }
        public Games Games { get; set; }
    }
}
