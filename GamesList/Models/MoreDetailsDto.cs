namespace GamesList.Models
{
    public class MoreDetailsDto
    {
        public List<string[]> Developers { get; set; }
        public List<string> Genres { get; set; }
        public List<string> Platforms { get; set; }
        public RatingDto Ratings { get; set; }
    }
}
