using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rtl.MazeScrapper.Application.Queries;
using Rtl.TvMaze.Persistence;

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

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetTvShowsQueryHandler).Assembly));
        //builder.Services.AddDbContext<TvMazeDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));
        var dbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "havij.db");
        builder.Services.AddDbContext<TvMazeDbContext>(opt => opt.UseSqlite($"Data Source={dbPath}"));
        var app = builder.Build();


        //using (var context = app.Services.GetRequiredService<TvMazeDbContext>())
        //{
        //    context.Database.EnsureCreated();
        //    context.Database.Migrate();
        //}


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