using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NintendoGames.Models.UserRequestModels;
using NintendoGames.Services.UserRequestService;

namespace NintendoGames.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserRequestController : ControllerBase
    {
        private readonly IUserRequestService _userRequest;

        public UserRequestController(IUserRequestService userRequest)
        {
            _userRequest = userRequest;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserRequestDto>>> GetAllUserRequests()
        {
            var list = await _userRequest.GetAllUserRequests();

            return Ok(list);
        }
    }
}
