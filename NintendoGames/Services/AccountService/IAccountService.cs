using NintendoGames.Models.AccountModels;

namespace NintendoGames.Services.AccountService
{
    public interface IAccountService
    {
        Task Register(CreateAccountDto createAccountDto);
        Task<string> GenerateJwt(LoginDto loginDto);
    }
}
