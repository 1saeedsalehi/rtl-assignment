using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rtl.MazeScrapper;
using Rtl.MazeScrapper.Application.BackgroundJobs;
using Rtl.TvMaze.Persistence;

namespace Rtl.TvMaze.Scrapper;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder();

        builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();


        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services, context.Configuration);
            })
            .Build();

        await host.RunAsync();

    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<ScrapBackgroundJob>();

        services.AddTvMazeClient(configuration);
        var dbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "havij.db");
        services.AddDbContext<TvMazeDbContext>(opt => opt.UseSqlite($"Data Source={dbPath}"));
    }


}