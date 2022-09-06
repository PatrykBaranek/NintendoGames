using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Exceptions;
using NintendoGames.Models.DataScraperModels;

namespace NintendoGames.Services.DataScraperService
{
    public partial class DataScraperService
    {
        
        private readonly IMapper _mapper;

        private static List<Game> _gamesFromDb = new();

       

        public async Task<List<ScrapedGameFromDbDto>> GetListFromDb()
        {
            await GetGamesFromDatabase();

            if (_gamesFromDb.Count == 0)
                throw new NoContentException("Not found games");

            return _mapper.Map<List<ScrapedGameFromDbDto>>(_gamesFromDb);
        }

        public async Task<List<ScrapedGameFromDbDto>> GetGamesByNameFromDb(string gameName)
        {
            await GetGamesFromDatabase();

            var queryString = gameName.Trim().ToLower();

            var result = _gamesFromDb.Where(g => g.Title.ToLower().Contains(queryString)).OrderBy(g => g.ReleaseDate).ToList();

            if (result.Count == 0)
                throw new NotFoundException($"Not found games named {queryString}");

            return _mapper.Map<List<ScrapedGameFromDbDto>>(result);
        }

        private async Task<List<Game>> GetGamesFromDatabase()
        {
            if (!_dbContext.Game.Any()) return null;

            var gamesList = await _dbContext.Game
                .Include(g => g.Developers)
                .Include(g => g.Genres)
                .Include(g => g.Rating)
                .ToListAsync();

            _gamesFromDb = gamesList;

            return _gamesFromDb;
        }
    }

}
