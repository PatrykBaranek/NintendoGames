using GamesList.Models;

namespace NintendoGames.Models
{
    public class GameDto
    {
        public string GameTitle { get; set; }
        public string ImageUrl { get; set; }
        public string ReleaseDate { get; set; }
        public MoreDetailsDto MoreDetails { get; set; }
    }
}
