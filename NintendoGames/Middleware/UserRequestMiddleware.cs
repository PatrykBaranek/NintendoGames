using System.Security.Claims;
using NintendoGames.Entities;
using NintendoGames.Services;

namespace NintendoGames.Middleware
{
    public class UserRequestMiddleware : IMiddleware
    {
        private readonly NintendoDbContext _dbContext;
        //private readonly UserContextService _userContextService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRequestMiddleware(NintendoDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            //_userContextService = userContextService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next.Invoke(context);

            var newRequest = new UserRequest
            {
                UserEmail = _httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                UserRole = _httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Role).Value,
                Request = context.Request.Method,
                RequestUrl = context.Request.Path,
                RequestDate = DateTime.Now
            };

            await _dbContext.UserRequest.AddAsync(newRequest);
            await _dbContext.SaveChangesAsync();
        }
    }
}
