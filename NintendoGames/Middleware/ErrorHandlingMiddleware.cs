using NintendoGames.Exceptions;

namespace NintendoGames.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
               await next.Invoke(context);
            }
            catch (BadRequestException e)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"Bad Request {e.Message}");
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
