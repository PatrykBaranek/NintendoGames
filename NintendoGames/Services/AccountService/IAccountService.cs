using NintendoGames.Models.AccountModels;

namespace NintendoGames.Services.AccountService
{
    public interface IAccountService
    {
        Task Register(CreateUserDto createUserDto);
        Task<string> GenerateJwt(LoginDto loginDto);
    }
}
