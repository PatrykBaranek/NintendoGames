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
            catch (NotFoundException e)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(e.Message);
            }
            catch (NoContentException e)
            {
                context.Response.StatusCode = 204;
                await context.Response.WriteAsync(e.Message);
            }
            catch (ForbiddenException e)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(e.Message);
            }
            catch (UnauthorizedException e)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(e.Message);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
