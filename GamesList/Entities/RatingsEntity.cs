using NintendoGames.Entities;

namespace GamesList.Entities
{
    public class RatingsEntity
    {
        public Guid Id { get; set; }
        public int MetacriticCriticRating { get; set; }
        public double MetacriticUserScoreRating { get; set; }

        public int GameId { get; set; }
        public GamesEntity Games { get; set; }
    }
}
