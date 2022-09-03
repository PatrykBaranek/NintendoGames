using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Exceptions;
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

        public async Task<List<GameDto>> GetAllGames()
        {
            var gamesList = await GetGamesFromDatabase();

            return gamesList;
        }

        public async Task<List<GameDto>> GetGamesByQuery(string gameName)
        {
            var games = await GetGamesFromDatabase();

            var queryString = gameName.Trim().ToLower();

            var result = games.Where(g => g.Title.ToLower().Contains(queryString)).OrderBy(g => g.ReleaseDate).ToList();

            if (result.Count == 0)
                throw new NotFoundException($"Not found games like {queryString}");

            return result;
        }


        public async Task DeleteGame(Guid gameId)
        {
            var gameToDelete = await _dbContext.Game
                .Include(g => g.Developers)
                .Include(g => g.Genres)
                .Include(g => g.Rating)
                .FirstAsync(g=> g.Id == gameId);

            if (gameToDelete is null)
                throw new NotFoundException("Not found game to delete");

            _dbContext.Remove(gameToDelete);
            await _dbContext.SaveChangesAsync();
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
