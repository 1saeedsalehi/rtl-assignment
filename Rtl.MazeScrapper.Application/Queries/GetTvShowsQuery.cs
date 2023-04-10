using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Rtl.MazeScrapper.Domain;
using Rtl.MazeScrapper.Domain.Dtos;
using Rtl.MazeScrapper.Domain.Entities;
using System.Text.Json;

namespace Rtl.MazeScrapper.Application.Queries;

public class GetTvShowsQuery : IRequest<PagedResponse<IEnumerable<TvShow>>>
{
    public int Page { get; set; } = 1;
    public int ItemCount { get; set; } = 10;

}

public class GetTvShowsQueryHandler : IRequestHandler<GetTvShowsQuery, PagedResponse<IEnumerable<TvShow>>>
{
    private readonly IDistributedCache _distributedCache;

    public GetTvShowsQueryHandler(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<PagedResponse<IEnumerable<TvShow>>> Handle(GetTvShowsQuery request,
        CancellationToken cancellationToken)
    {
        var allShowIndexes = await _distributedCache.GetStringAsync(Constants.AllShowsCacheKey, cancellationToken);
        if (allShowIndexes is null)
            return default;

        var showIndexes = JsonSerializer.Deserialize<IEnumerable<int>>(allShowIndexes);

        var shows = showIndexes
            .Skip(request.Page * request.ItemCount)
            .Take(request.ItemCount)
            .Select(async showId => await GetTVShowAsync(showId, cancellationToken))
                .Select(task => task.Result)
            .AsEnumerable();

        return new PagedResponse<IEnumerable<TvShow>>(shows, showIndexes.Count());
    }

    private async Task<TvShow> GetTVShowAsync(int showId, CancellationToken cancellationToken = default)
    {
        var cached = await _distributedCache.GetStringAsync(Constants.GetShowCacheKey(showId), cancellationToken);
        if (cached is null || string.IsNullOrEmpty(cached))
            return default;

        return JsonSerializer.Deserialize<TvShow>(cached);
    }
}
