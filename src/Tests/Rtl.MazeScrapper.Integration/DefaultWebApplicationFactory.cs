using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Rtl.TvMaze.Persistence;
using Rtl.TvMaze.Presentation;
using Xunit.Abstractions;

namespace Rtl.MazeScrapper.Integration;

public class DefaultWebApplicationFactory : WebApplicationFactory<Program>, ITestOutputHelperAccessor
{
    public ITestOutputHelper? OutputHelper { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureLogging(logging => logging
            .ClearProviders()
            .SetMinimumLevel(LogLevel.Error)
            .AddFilter(logLevel => logLevel >= LogLevel.Error)
            .AddConsole()
            .AddXUnit(this));

        builder.ConfigureTestServices(services =>
        {
            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            inMemorySqlite.Open();

            services.RemoveAll(typeof(DbContextOptions<TvMazeDbContext>));
            services.AddDbContextPool<TvMazeDbContext>(opt =>
            {
                opt.UseSqlite(inMemorySqlite);
            });
        });
    }
}
