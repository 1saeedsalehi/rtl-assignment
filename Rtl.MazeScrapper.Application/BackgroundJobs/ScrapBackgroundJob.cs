using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rtl.MazeScrapper.Application.HttpClients;
using Rtl.MazeScrapper.Domain;
using Rtl.MazeScrapper.Domain.Entities;
using System.Text.Json;

namespace Rtl.MazeScrapper.Application.BackgroundJobs;


public class ScrapBackgroundJob : BackgroundService
{
    private readonly ILogger<ScrapBackgroundJob> _logger;
    private readonly ITvMazeHttpClient _tvMazeHttpClient;
    private readonly IDistributedCache _distributedCache;

    public ScrapBackgroundJob(
        ILogger<ScrapBackgroundJob> logger,
        ITvMazeHttpClient tvMazeHttpClient,
        IDistributedCache cache)
    {
        _logger = logger;
        _tvMazeHttpClient = tvMazeHttpClient;
        _distributedCache = cache;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        _logger.LogInformation("start getting tv shows");

        List<int> showIds = new();
        int pageNumber = 0;
        bool isFinished = false;

        var cached = await _distributedCache.GetStringAsync(Constants.AllShowsCacheKey, stoppingToken);


        if (cached is not null)
        {
            var cachedData = JsonSerializer.Deserialize<(List<int> showIds, int lastPage)>(cached);
            var response = await _tvMazeHttpClient.GetShowsAsync(cachedData.lastPage, stoppingToken);

            var hasNewItem = response?.Select(x => x.Id).Max() > cachedData.showIds.Max();
            if (response is not null && hasNewItem)
            {
                cachedData.showIds.AddRange(response.Select(x => x.Id));
                pageNumber++;
                isFinished = false;
            }
        }

        do
        {
            _logger.LogInformation($"scrapping tv shows page {pageNumber}");

            var response = await _tvMazeHttpClient.GetShowsAsync(pageNumber, stoppingToken);
            if (response is null)
            {
                _logger.LogInformation($"scrapping all shows finished - {pageNumber} pages");
                isFinished = true;
                continue;
            }
            showIds.AddRange(response.Select(x => x.Id));
            pageNumber++;
        }
        while (!isFinished);

        _logger.LogInformation("start processing tv shows detail");

        showIds = showIds.Distinct().OrderBy(x => x).ToList();

        (IEnumerable<int> showIds, int lastPage) dataToPersist = (showIds.ToList(), pageNumber);

        await _distributedCache.SetStringAsync(Constants.AllShowsCacheKey,
            JsonSerializer.Serialize(dataToPersist),
            stoppingToken);


        await Task.Run(() => Parallel.ForEach(showIds, async showId =>
        {
            await GetShowDetail(showId, stoppingToken);
        }));

    }

    private async Task GetShowDetail(int showId, CancellationToken cancellationToken)
    {

        _logger.LogInformation($"getting show details for id {showId}");

        var showDetail = await _tvMazeHttpClient.GetShowDetailAsync(showId, cancellationToken);
        var tvShow = new TvShow(
            showDetail.Id,
            showDetail.Name,
            showDetail.Embedded.Cast
                .Select(cast =>
                    new Artist(
                        cast.Person.Id,
                        cast.Person.Name,
                        cast.Person.Birthday))
                .OrderByDescending(p => p.Birthday)
                .ToList());

        await _distributedCache.SetStringAsync(
            Constants.GetShowCacheKey(tvShow.Id),
            JsonSerializer.Serialize(tvShow),
            cancellationToken);
    }
}
