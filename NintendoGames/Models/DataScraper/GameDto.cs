namespace NintendoGames.Models.DataScraper
{
    public class GameDto
    {
        public string Id { get; set; }
        public string GameTitle { get; set; }
        public string ImageUrl { get; set; }
        public string ReleaseDate { get; set; }
        public MoreDetailsDto MoreDetails { get; set; }
    }
}
