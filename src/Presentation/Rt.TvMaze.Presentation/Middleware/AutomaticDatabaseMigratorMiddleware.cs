using Microsoft.EntityFrameworkCore;
using Rtl.TvMaze.Persistence;

namespace Rt.TvMaze.Presentation.Middleware;

public class AutomaticDatabaseMigratorMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        var db = httpContext.RequestServices.GetRequiredService<TvMazeDbContext>();

        if (db.Database.IsRelational())
        {
            await db.Database.MigrateAsync(httpContext.RequestAborted);
        }

        await next(httpContext);
    }
}
