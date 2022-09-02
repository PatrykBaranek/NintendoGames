using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Models.Games;

namespace NintendoGames.Services.Games
{
    public class GamesService : IGamesService
    {
        private readonly NintendoDbContext _dbContext;
        private readonly IMapper _mapper;

        public GamesService(NintendoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task AddGame()
        {
            throw new NotImplementedException();
        }

        public async Task<List<GameDto>> GetAllGames()
        {
            var gamesList = await GetGamesFromDatabase();

            return gamesList;

        }

        public async Task<List<GameDto>> GetGame(string gameName)
        {
            var games = await GetGamesFromDatabase();

            return games.Where(g => g.Title == gameName).ToList();
        }

        public Task UpdateGame()
        {
            throw new NotImplementedException();
        }

        public Task DeleteGame()
        {
            throw new NotImplementedException();
        }

        private async Task<List<GameDto>> GetGamesFromDatabase()
        {
            var gamesFromDb = await _dbContext.Game
                .Include(g => g.Developers)
                .Include(g => g.Rating)
                .Include(g => g.Genres)
                .ToListAsync();

            var gamesList = _mapper.Map<List<GameDto>>(gamesFromDb);


            return gamesList;
        }
    }
}
