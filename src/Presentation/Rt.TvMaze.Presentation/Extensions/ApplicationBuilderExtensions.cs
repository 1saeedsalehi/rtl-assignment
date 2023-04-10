using Rt.TvMaze.Presentation.Middleware;

namespace Rt.TvMaze.Presentation.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseAutomaticDatabaseMigrator(this IApplicationBuilder app)
    {
        app.UseMiddleware<AutomaticDatabaseMigratorMiddleware>();
    }
}
