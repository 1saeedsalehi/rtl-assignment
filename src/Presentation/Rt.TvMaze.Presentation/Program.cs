using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rt.TvMaze.Presentation.Extensions;
using Rt.TvMaze.Presentation.Middleware;
using Rtl.MazeScrapper.Application.Queries;
using Rtl.TvMaze.Persistence;
using System.Text.Json.Serialization;

namespace Rtl.MazeScrapper;

public class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetTvShowsQueryHandler).Assembly));

        var dbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TvMaze.db");
        string? connectionString = string.Format(builder.Configuration.GetConnectionString("Default"),dbPath);
        builder.Services.AddDbContext<TvMazeDbContext>(opt => opt.UseSqlite(connectionString));


        builder.Services.AddScoped<AutomaticDatabaseMigratorMiddleware>();

        var app = builder.Build();


        // Configure the HTTP request pipeline.

        app.UseAutomaticDatabaseMigrator();

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