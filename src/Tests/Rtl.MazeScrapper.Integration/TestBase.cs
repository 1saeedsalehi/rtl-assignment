using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Rtl.TvMaze.Persistence;

namespace Rtl.MazeScrapper.Integration;

public class TestBase
{
    internal const string TvShowEndpoint = "/api/TvShow";

    internal DefaultWebApplicationFactory apiFactory;
    internal IServiceScopeFactory _scopeFactory;
    internal IConfiguration _configuration;

    public TestBase()
    {
        apiFactory = new DefaultWebApplicationFactory();
        _scopeFactory = apiFactory.Services.GetRequiredService<IServiceScopeFactory>();
        _configuration = apiFactory.Services.GetRequiredService<IConfiguration>();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public async Task InsertToDatabase<TEntity>(List<TEntity> entities) where TEntity : class
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<TvMazeDbContext>();
            context.AddRange(entities);
            await context.SaveChangesAsync();
        }

    }

}
