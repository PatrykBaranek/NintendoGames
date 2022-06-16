using GamesList.Entities;

namespace NintendoGames.Entities
{
    public class GamesEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public Guid PriceId { get; set; }
        public PricesEntity Prices { get; set; }

        public Guid RatingId { get; set; }
        public RatingsEntity Ratings { get; set; }
    }
}
