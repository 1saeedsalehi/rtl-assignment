using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rtl.TvMaze.Persistence;

namespace Rtl.TvMaze.Scrapper.Middleware;

internal class AutomaticDatabaseMigratorMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        var db = httpContext.RequestServices.GetRequiredService<TvMazeDbContext>();

        if (await db.Database.EnsureCreatedAsync())
        {
            await db.Database.MigrateAsync(httpContext.RequestAborted);
        }

        await next(httpContext);
    }
}
