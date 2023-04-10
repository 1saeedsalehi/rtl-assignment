using Rtl.TvMaze.Presentation.Middleware;

namespace Rtl.TvMaze.Presentation.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseAutomaticDatabaseMigrator(this IApplicationBuilder app)
    {
        app.UseMiddleware<AutomaticDatabaseMigratorMiddleware>();
    }
}
