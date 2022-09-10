using System.Security.Claims;
using NintendoGames.Entities;

namespace NintendoGames.Middleware
{
    public class UserRequestMiddleware : IMiddleware
    {
        private readonly NintendoDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _registerUrl;
        public UserRequestMiddleware(NintendoDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _registerUrl = context.Request.Path;

            await next.Invoke(context);

            if (_registerUrl is not (@"/api/account/register" or @"/api/account/login"))
            {
                var newRequest = new UserRequest
                {
                    UserEmail = _httpContextAccessor.HttpContext?.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                    UserRole = _httpContextAccessor.HttpContext?.User.FindFirst(c => c.Type == ClaimTypes.Role).Value,
                    Request = context.Request.Method,
                    RequestUrl = context.Request.Path,
                    StatusCode = context.Request.HttpContext.Response.StatusCode,
                    RequestDate = DateTime.Now
                };

                await _dbContext.UserRequest.AddAsync(newRequest);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
