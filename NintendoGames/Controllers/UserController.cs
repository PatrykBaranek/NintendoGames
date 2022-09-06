using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.UserModels;
using NintendoGames.Services.UserService;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(CreateUserDto createUserDto)
        {
            await _userService.Register(createUserDto);

            return Ok();
        }
    }
}
