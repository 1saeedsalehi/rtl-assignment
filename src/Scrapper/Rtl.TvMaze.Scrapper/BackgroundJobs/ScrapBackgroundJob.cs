using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rtl.TvMaze.Domain.Entities;
using Rtl.TvMaze.Infrastructure;
using Rtl.TvMaze.Persistence;
using Rtl.TvMaze.Scrapper.HttpClients;

namespace Rtl.TvMaze.Scrapper.BackgroundJobs;


public class ScrapBackgroundJob : BackgroundService
{
    private readonly ILogger<ScrapBackgroundJob> _logger;
    private readonly ITvMazeHttpClient _tvMazeHttpClient;
    private readonly TvMazeDbContext _dbContext;


    public ScrapBackgroundJob(
        ILogger<ScrapBackgroundJob> logger,
        ITvMazeHttpClient tvMazeHttpClient,
        TvMazeDbContext dbContext)
    {
        _logger = logger;
        _tvMazeHttpClient = tvMazeHttpClient;
        _dbContext = dbContext;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        _logger.LogInformation("start getting tv shows");


        var LastId = _dbContext.TvShows.Any() ? _dbContext.TvShows.AsNoTracking().Max(show => show.Id) : 0;
        var pageNumber = (int)Math.Ceiling((double)LastId / Constants.PageSize);

        List<TvShow> tvShows = new();

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"scrapping tv shows page {pageNumber}");

            var response = await _tvMazeHttpClient.GetShowsAsync(pageNumber, stoppingToken);

            if (response is null)
            {
                //finished scrapping...
                continue;
            }

            pageNumber++;

            _logger.LogInformation("start processing tv shows detail");

            var showIds = response.Select(x => x.Id).Distinct().OrderBy(x => x).ToList();

            var allTasks = showIds.Select(showId => GetShowDetail(showId, stoppingToken));

            var allTvShows = await Task.WhenAll(allTasks);


            _dbContext.TvShows.AddRange(allTvShows);


            await _dbContext.SaveChangesAsync(stoppingToken);

        }

    }

    private async Task<TvShow> GetShowDetail(int showId, CancellationToken stoppingToken)
    {
        _logger.LogInformation($"getting show details for id {showId}");

        var showDetail = await _tvMazeHttpClient.GetShowDetailAsync(showId, stoppingToken);

        var tvShow = new TvShow(
            showDetail.Name,
            showDetail.Embedded.Cast
                .Select(cast =>
                    new Artist(
                        showId,
                        cast.Person.Name,
                        cast.Person.Birthday))
                .OrderByDescending(p => p.Birthday)
                .ToList());


        return tvShow;

    }
}
