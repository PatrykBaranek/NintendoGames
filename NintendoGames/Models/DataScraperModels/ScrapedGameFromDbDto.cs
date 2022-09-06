﻿namespace NintendoGames.Models.DataScraperModels
{
    public class ScrapedGameFromDbDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ImageUrl { get; set; }

        public List<string> GenreName { get; set; }
        public List<string> DeveloperName { get; set; }

        public int CriticRating { get; set; }
        public double? UserScore { get; set; }
        public bool IsMustPlay { get; set; }
    }
}
