using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NintendoGames.Entities;
using NintendoGames.Exceptions;
using NintendoGames.Models.GamesModels;
using NintendoGames.Models.WishListModels;

namespace NintendoGames.Services.WishListService
{
    public class WishListService : IWishListService
    {
        private readonly NintendoDbContext _dbContext;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public WishListService(NintendoDbContext dbContext, IUserContextService userContextService, IMapper mapper)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<List<GameDto>> ShowAllGamesUserWishList()
        {
            var userId = _userContextService.GetUserId;

            if (userId is null)
                throw new UnauthorizedException("Unauthorized");

            var wishList = await _dbContext.WishList.FirstOrDefaultAsync(w => w.UserId == userId);

            var gamesWishList = await _dbContext.GameWishList.Where(gw => gw.WishListId == wishList.Id).ToListAsync();

            var gamesIds = gamesWishList.Select(g => g.GameId).ToList();

            var gamesList = new List<Game>();

            foreach (var gameId in gamesIds)
            {
                gamesList.Add(
                    await _dbContext.Game
                    .Include(g => g.Rating)
                    .Include(g => g.Developers)
                    .Include(g => g.Genres)
                    .FirstOrDefaultAsync(g => g.Id == gameId)
                    );
            }

            var wishListDto = _mapper.Map<List<GameDto>>(gamesList);

            return wishListDto;
        }

        public async Task AddGameToWishList(AddGameToWishListDto addGameToWishListDto)
        {
            var gameToAdd = await _dbContext.Game.FirstOrDefaultAsync(g => g.Title.ToLower().Contains(addGameToWishListDto.GameName.ToLower().Trim()));

            if (gameToAdd is null)
                throw new NotFoundException("Not found game");

            var userId = _userContextService.GetUserId;

            var wishList = await _dbContext.WishList.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishList is null)
                throw new UnauthorizedException("Unauthorized");

            var gamesInWishList = await _dbContext.GameWishList.Where(gw => gw.WishListId == wishList.Id).ToListAsync();

            if (gamesInWishList.Any(g => g.GameId == gameToAdd.Id))
                throw new BadRequestException("Game already exist in your WishList");

            var addGameToWishList = new GameWishList
            {
                WishListId = wishList.Id,
                GameId = gameToAdd.Id
            };

            await _dbContext.GameWishList.AddAsync(addGameToWishList);
            await _dbContext.SaveChangesAsync();
        }
    }
}
