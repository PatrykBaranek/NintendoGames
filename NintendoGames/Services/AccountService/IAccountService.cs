using NintendoGames.Models.UserModels;

namespace NintendoGames.Services.AccountService
{
    public interface IAccountService
    {
        Task Register(CreateUserDto createUserDto);
        Task<string> GenerateJwt(LoginDto loginDto);
    }
}
