using GamesList.Entities;

namespace NintendoGames.Entities
{
    public class GamesEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Companies { get; set; }

        public int PriceId { get; set; }
        public PricesEntity Prices { get; set; }

        public int RatingId { get; set; }
        public RatingsEntity Ratings { get; set; }
    }
}
