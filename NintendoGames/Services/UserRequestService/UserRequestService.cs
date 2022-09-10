using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Exceptions;
using NintendoGames.Models.UserRequestModels;

namespace NintendoGames.Services.UserRequestService
{
    public class UserRequestService : IUserRequestService
    {
        private readonly NintendoDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserRequestService(NintendoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<List<UserRequestDto>> GetAllUserRequests()
        {
            var userRequests = await _dbContext.UserRequest.ToListAsync();

            if (userRequests.Count == 0)
                throw new NotFoundException("Not found any user requests");

            var mappedList = _mapper.Map<List<UserRequestDto>>(userRequests);

            return mappedList;
        }
    }
}
