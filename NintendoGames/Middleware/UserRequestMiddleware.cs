using System.Security.Claims;
using NintendoGames.Entities;

namespace NintendoGames.Middleware
{
    public class UserRequestMiddleware : IMiddleware
    {
        private readonly NintendoDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRequestMiddleware(NintendoDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next.Invoke(context);

            var user = _httpContextAccessor.HttpContext?.User;

            if (user.Identity.IsAuthenticated)
            {
                var newRequest = new UserRequest
                {
                    UserEmail = _httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                    UserRole = _httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Role).Value,
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
