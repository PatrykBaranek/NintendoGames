using NintendoGames.Entities;
using NintendoGames.Exceptions;

namespace NintendoGames.Services.DataScraperService
{
    public partial class DataScraperService
    {
        private static readonly List<Developers> Developers = new();
        private static readonly List<Genres> Genres = new();
        private static readonly List<Game> Games = new();
        private static readonly List<Rating> Ratings = new();


        public async Task PostGamesToDatabase()
        {
            if (GamesList.Count == 0)
                throw new NotFoundException("Not found games");

            await DeleteDuplicateDataFromList();

            if (GamesList.Count == 0)
                throw new NotFoundException("Not found games to add");

            GameFormat();

            await _dbContext.Game.AddRangeAsync(Games);
            await _dbContext.Rating.AddRangeAsync(Ratings);
            await _dbContext.Developers.AddRangeAsync(Developers);
            await _dbContext.Genres.AddRangeAsync(Genres);

            await _dbContext.SaveChangesAsync();
        }


        private async Task DeleteDuplicateDataFromList()
        {
            if (await GetGamesFromDatabase() == null)
            {
                return;
            }

            var gamesToRemove = GamesList.Where(gameDto => _gamesFromDb.Any(g => g.ImageUrl == gameDto.ImageUrl)).ToList();

            GamesList.RemoveAll(g => gamesToRemove.Contains(g));
        }

        private static void GameFormat()
        {
            Games.Clear();
            Ratings.Clear();
            Developers.Clear();
            Genres.Clear();

            foreach (var gameDto in GamesList)
            {
                var gameToDb = new Game
                {
                    Id = Guid.NewGuid(),
                    Title = gameDto.GameTitle,
                    ImageUrl = gameDto.ImageUrl,
                    ReleaseDate = FormatReleaseDate(gameDto.ReleaseDate),
                };

                var ratingToGame = new Rating
                {
                    Id = Guid.NewGuid(),
                    CriticRating = FormatCriticReview(gameDto.MoreDetails.Ratings.CriticRating),
                    UserScore = FormatUserScore(gameDto.MoreDetails.Ratings.UserScore),
                    IsMustPlay = gameDto.MoreDetails.Ratings.IsMustPlay,
                    GameId = gameToDb.Id
                };

                gameToDb.RatingId = ratingToGame.Id;

                Games.Add(gameToDb);
                Ratings.Add(ratingToGame);

                DevelopersFormat(gameDto.MoreDetails.Developers, gameToDb.Id);
                GenresFormat(gameDto.MoreDetails.Genres, gameToDb.Id);
            }
        }

        private static void GenresFormat(List<string> genresDto, Guid gameId)
        {
            foreach (var genre in genresDto)
            {
                if (genre == string.Empty)
                {
                    continue;
                }

                var genresToGame = new Genres
                {
                    Id = Guid.NewGuid(),
                    Name = genre,
                    GameId = gameId
                };

                Genres.Add(genresToGame);
            }
        }

        private static void DevelopersFormat(List<string[]> developersDto, Guid gameId)
        {
            foreach (var developers in developersDto)
            {
                foreach (var developer in developers)
                {
                    var developerToGame = new Developers
                    {
                        Id = Guid.NewGuid(),
                        Name = developer,
                        GameId = gameId
                    };

                    Developers.Add(developerToGame);
                }
            }
        }

        private static int FormatCriticReview(string criticRating)
        {
            return criticRating is "" ? 0 : int.Parse(criticRating);
        }

        private static double FormatUserScore(string userScore)
        {
            if (userScore is "" or "tbd")
            {
                return 0;
            }

            var userScoreStringFormat = userScore.Replace(".", ",");

            return double.Parse(userScoreStringFormat);
        }

        private static DateTime FormatReleaseDate(string dateToFormat)
        {
            var dateAsArrayString = dateToFormat.Split(' ');

            var month = dateAsArrayString[0];
            var day = dateAsArrayString[1].Replace(",", "");
            var year = dateAsArrayString[2];

            return DateTime.Parse(string.Join("/", month, day, year));
        }
    }
}
