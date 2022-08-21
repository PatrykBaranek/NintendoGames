using NintendoGames.Entities;

namespace NintendoGames.Services.Games
{
    public class GamesService : IGamesService
    {
        private readonly NintendoDbContext _dbContext;

        public GamesService(NintendoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        

    }
}
