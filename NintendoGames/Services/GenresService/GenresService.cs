using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Exceptions;
using NintendoGames.Models.GenresModels;

namespace NintendoGames.Services.GenresService
{
    public class GenresService : IGenresService
    {
        private readonly NintendoDbContext _dbContext;
        private readonly IMapper _mapper;

        public GenresService(NintendoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<GenresDto>> GetAllGameGenres(Guid gameId)
        {
            var gameGenres = await GetAllGameGenresFromDatabase(gameId);

            return _mapper.Map<List<GenresDto>>(gameGenres);
        }


        public async Task AddGenre(Guid gameId, AddGenreDto addGenreDto)
        {
            var gameGenres = await GetAllGameGenresFromDatabase(gameId);

            if (gameGenres.Any(ge =>
                    string.Equals(ge.Name, addGenreDto.GenreName, StringComparison.CurrentCultureIgnoreCase)))
                throw new BadRequestException("Game already have this genre");

            var genreToAdd = _mapper.Map<Genres>(addGenreDto);

            genreToAdd.GameId = gameId;
            genreToAdd.Id = Guid.NewGuid();

            await _dbContext.Genres.AddAsync(genreToAdd);
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteGenre(Guid gameId, DeleteGenreDto deleteGenreDto)
        {
            var gameGenres = await GetAllGameGenresFromDatabase(gameId);

            if (deleteGenreDto.GenreId is null && deleteGenreDto.GenreName == string.Empty)
                throw new BadRequestException("One of params have to be filled");

            if (deleteGenreDto.GenreId is not null)
            {
                var genreToDelete = gameGenres.FirstOrDefault(d => d.Id == deleteGenreDto.GenreId);

                if (genreToDelete is null)
                    throw new NotFoundException("Developer not found");

                _dbContext.Genres.Remove(genreToDelete);
                await _dbContext.SaveChangesAsync();
                return;
            }

            if (deleteGenreDto.GenreName != string.Empty)
            {
                var genreToDelete = gameGenres.FirstOrDefault(d => string.Equals(d.Name, deleteGenreDto.GenreName, StringComparison.CurrentCultureIgnoreCase));

                if (genreToDelete is null)
                    throw new NotFoundException("Developer not found");

                _dbContext.Genres.Remove(genreToDelete);
                await _dbContext.SaveChangesAsync();
            }

        }

        private async Task<List<Genres>> GetAllGameGenresFromDatabase(Guid gameId)
        {
            var gameGenres = await _dbContext.Genres.Where(g => g.GameId == gameId).ToListAsync();

            if (gameGenres.Count == 0)
                throw new NotFoundException("Not found game");

            return gameGenres;
        }
    }
}
