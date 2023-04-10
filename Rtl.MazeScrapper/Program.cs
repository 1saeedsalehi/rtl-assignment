using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Rtl.MazeScrapper.Application.BackgroundJobs;
using Rtl.MazeScrapper.Application.Queries;

namespace Rtl.MazeScrapper;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHostedService<ScrapBackgroundJob>();

        //use AddDistributedMemoryCache as a temporary persistence solution for demo.
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetTvShowsQueryHandler).Assembly));
        builder.Services.AddTvMazeClient(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}