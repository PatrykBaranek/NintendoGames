using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Exceptions;
using NintendoGames.Models.RatingModels;

namespace NintendoGames.Services.RatingService
{
    public class RatingService : IRatingService
    {
        private readonly NintendoDbContext _dbContext;
        private readonly IMapper _mapper;

        public RatingService(NintendoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task UpdateUserScore(Guid gameId, UpdateUserScoreDto updateUserScoreDto)
        {
            var ratingToUpdate = await _dbContext.Rating.FirstAsync(r => r.GameId == gameId);

            if (ratingToUpdate is null)
                throw new NotFoundException("Not found game");

            ratingToUpdate.UserScore = updateUserScoreDto.UserScore;

            _dbContext.Rating.Update(ratingToUpdate);
            await _dbContext.SaveChangesAsync();
        }
    }
}
