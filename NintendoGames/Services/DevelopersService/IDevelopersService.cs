using NintendoGames.Models.DevelopersModels;

namespace NintendoGames.Services.DevelopersService
{
    public interface IDevelopersService
    {
        Task AddDeveloper(Guid gameId, AddDeveloperDto addDeveloperDto);
        Task DeleteDeveloper(Guid gameId, DeleteDeveloperDto deleteDeveloperDto);
    }
}
