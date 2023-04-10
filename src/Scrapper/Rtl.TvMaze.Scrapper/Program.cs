using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rtl.MazeScrapper;
using Rtl.MazeScrapper.Application.BackgroundJobs;
using Rtl.TvMaze.Persistence;
using Rtl.TvMaze.Scrapper.Middleware;

namespace Rtl.TvMaze.Scrapper;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder();

        builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();


        var app = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services, context.Configuration);
            })
            .Build();

        var db = app.Services.GetRequiredService<TvMazeDbContext>();

        if (db.Database.IsRelational())
        {
            await db.Database.MigrateAsync();
        }

        await app.RunAsync();

    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<ScrapBackgroundJob>();

        services.AddTvMazeClient(configuration);


        var dbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TvMaze.db");
        string? connectionString = string.Format(configuration.GetConnectionString("Default"), dbPath);
        services.AddDbContext<TvMazeDbContext>(opt => opt.UseSqlite(connectionString));

        services.AddScoped<AutomaticDatabaseMigratorMiddleware>();
    }


}