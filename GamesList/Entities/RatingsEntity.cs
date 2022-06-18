using NintendoGames.Entities;

namespace GamesList.Entities
{
    public class RatingsEntity
    {
        public Guid Id { get; set; }
        public int MetacriticCriticRating { get; set; }

        public Guid GameId { get; set; }
        public GamesEntity Games { get; set; }
    }
}
