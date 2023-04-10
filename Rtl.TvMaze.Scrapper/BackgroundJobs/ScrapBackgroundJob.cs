using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rtl.MazeScrapper.Application.HttpClients;
using Rtl.MazeScrapper.Domain;
using Rtl.MazeScrapper.Domain.Entities;
using Rtl.TvMaze.Persistence;

namespace Rtl.MazeScrapper.Application.BackgroundJobs;


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


        var LastId = _dbContext.TvShows.Any() ? _dbContext.TvShows.Max(show => show.Id) : 0;
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

            var a = await Task.WhenAll(allTasks);

            //await Task.Run(() => Parallel.ForEach(showIds, async showId =>
            //{
            //    tvShows.Add(await GetShowDetail(showId, stoppingToken));
            //}));



            //_dbContext.AddRange(tvShow);
            //await _dbContext.SaveChangesAsync(stoppingToken);

        }

    }

    private async Task<TvShow> GetShowDetail(int showId, CancellationToken stoppingToken)
    {
        _logger.LogInformation($"getting show details for id {showId}");

        var showDetail = await _tvMazeHttpClient.GetShowDetailAsync(showId, stoppingToken);

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


        return tvShow;

    }
}
