using NintendoGames.Models.UserModels;

namespace NintendoGames.Services.UserService
{
    public interface IUserService
    {
        Task Register(CreateUserDto createUserDto);
    }
}
