using NintendoGames.Models.GenresModels;

namespace NintendoGames.Services.GenresService
{
    public interface IGenresService
    {
        Task AddGenre(Guid gameId, AddGenreDto addGenreDto);
        Task DeleteGenre(Guid gameId, DeleteGenreDto deleteGenreDto);
        Task<List<GenresDto>> GetAllGameGenres(Guid gameId);
    }
}
