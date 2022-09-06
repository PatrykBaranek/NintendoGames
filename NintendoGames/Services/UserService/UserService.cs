using AutoMapper;
using NintendoGames.Entities;
using NintendoGames.Models.UserModels;
using System.Collections.Generic;

namespace NintendoGames.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly NintendoDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(NintendoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Register(CreateUserDto createUserDto)
        {
            var userToAdd = _mapper.Map<User>(createUserDto);

            
            userToAdd.Id = Guid.NewGuid();
            userToAdd.RoleId = 1;

            var createWishList = new WishList
            {
                Id = Guid.NewGuid(),
                UserId = userToAdd.Id
            };

            userToAdd.WishListId = createWishList.Id;

            await _dbContext.User.AddAsync(userToAdd);
            await _dbContext.WishList.AddAsync(createWishList);
            await _dbContext.SaveChangesAsync();

        }
    }
}
