using NintendoGames.Models.UserRequestModels;

namespace NintendoGames.Services.UserRequestService
{
    public interface IUserRequestService
    {
        Task<List<UserRequestDto>> GetAllUserRequests();
    }
}
