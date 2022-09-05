using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Exceptions;
using NintendoGames.Models.DevelopersModels;

namespace NintendoGames.Services.DevelopersService
{
    public class DevelopersService : IDevelopersService
    {
        private readonly NintendoDbContext _dbContext;
        private readonly IMapper _mapper;


        public DevelopersService(NintendoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<DevelopersDto>> GetAllGameDevelopers(Guid gameId)
        {
            var developers = await GetAllGameDevelopersFromDatabase(gameId);

            return _mapper.Map<List<DevelopersDto>>(developers);
        }

        public async Task DeleteDeveloper(Guid gameId, DeleteDeveloperDto deleteDeveloperDto)
        {
            var developers = await GetAllGameDevelopersFromDatabase(gameId);

            if (deleteDeveloperDto.DeveloperId is null && deleteDeveloperDto.DeveloperName == string.Empty)
                throw new BadRequestException("One of params have to be filled");

            if (deleteDeveloperDto.DeveloperId is not null)
            {
                var developerToDelete = developers.FirstOrDefault(d => d.Id == deleteDeveloperDto.DeveloperId);

                if (developerToDelete is null)
                    throw new NotFoundException("Developer not found");

                _dbContext.Developers.Remove(developerToDelete);
                await _dbContext.SaveChangesAsync();
                return;
            }

            if (deleteDeveloperDto.DeveloperName != string.Empty)
            {
                var developerToDelete = developers.FirstOrDefault(d => string.Equals(d.Name, deleteDeveloperDto.DeveloperName, StringComparison.CurrentCultureIgnoreCase));

                if (developerToDelete is null)
                    throw new NotFoundException("Developer not found");

                _dbContext.Developers.Remove(developerToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task AddDeveloper(Guid gameId, AddDeveloperDto addDeveloperDto)
        {
            var developers = await GetAllGameDevelopersFromDatabase(gameId);

            if (developers.Any(d => string.Equals(d.Name, addDeveloperDto.DeveloperName, StringComparison.CurrentCultureIgnoreCase)))
                throw new BadRequestException("Developer already exist");

            var developerToAdd = _mapper.Map<Developers>(addDeveloperDto);

            developerToAdd.GameId = gameId;
            developerToAdd.Id = Guid.NewGuid();

            await _dbContext.Developers.AddAsync(developerToAdd);
            await _dbContext.SaveChangesAsync();
        }


        private async Task<List<Developers>> GetAllGameDevelopersFromDatabase(Guid gameId)
        {
            var developers = await _dbContext.Developers.Where(d => d.GameId == gameId).ToListAsync();

            if (developers.Count == 0)
                throw new NotFoundException("Not found game");

            return developers;
        }
    }
}
