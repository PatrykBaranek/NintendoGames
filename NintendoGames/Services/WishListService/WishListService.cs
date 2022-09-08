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

            var wishList = await GetUserWishList();

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
            var gameToAdd = await FindGameInDatabase(addGameToWishListDto.GameName);

            var wishList = await GetUserWishList();

            var addGameToWishList = new GameWishList
            {
                WishListId = wishList.Id,
                GameId = gameToAdd.Id
            };

            await _dbContext.GameWishList.AddAsync(addGameToWishList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteGameFromWishList(DeleteGameFromWishListDto deleteGameFromWishListDto)
        {
            var gameToDelete = await _dbContext.Game.FirstOrDefaultAsync(g => g.Title.ToLower().Contains(deleteGameFromWishListDto.GameName.ToLower().Trim()));

            if (gameToDelete is null)
                throw new NotFoundException("Not found game");

        }

        private async Task<Game> FindGameInDatabase(string gameName)
        {
            var game = await _dbContext.Game.FirstOrDefaultAsync(g => g.Title.ToLower().Contains(gameName.ToLower().Trim()));

            if (game is null)
                throw new NotFoundException("Not found game");

            return game;
        }

        private async Task<WishList> GetUserWishList()
        {
            var userId = _userContextService.GetUserId;

            if (userId is null)
                throw new UnauthorizedException("Unauthorized");

            var wishList = await _dbContext.WishList.FirstOrDefaultAsync(w => w.UserId == userId);

            return wishList;
        }
    }
}
