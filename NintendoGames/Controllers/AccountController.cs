using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.UserModels;
using NintendoGames.Services.AccountService;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(CreateUserDto createUserDto)
        {
            await _accountService.Register(createUserDto);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _accountService.GenerateJwt(loginDto);

            return Ok(token);
        }
    }
}
