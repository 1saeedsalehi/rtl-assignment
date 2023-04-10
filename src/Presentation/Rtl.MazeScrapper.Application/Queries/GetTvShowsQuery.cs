using MediatR;
using Microsoft.EntityFrameworkCore;
using Rtl.MazeScrapper.Domain.Dtos;
using Rtl.MazeScrapper.Domain.Entities;
using Rtl.TvMaze.Persistence;

namespace Rtl.MazeScrapper.Application.Queries;

public class GetTvShowsQuery : IRequest<PagedResponse<IEnumerable<TvShow>>>
{
    public int Page { get; set; } = 0;
    public int ItemCount { get; set; } = 10;

}

public class GetTvShowsQueryHandler : IRequestHandler<GetTvShowsQuery, PagedResponse<IEnumerable<TvShow>>>
{
    private readonly TvMazeDbContext _dbContext;

    public GetTvShowsQueryHandler(TvMazeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<IEnumerable<TvShow>>> Handle(GetTvShowsQuery request,
        CancellationToken cancellationToken)
    {
        var totalCount = await _dbContext.TvShows.AsNoTracking().CountAsync(cancellationToken);

        var tvShows = await _dbContext.TvShows
            .AsNoTracking()
            .Skip(request.Page * request.ItemCount)
            .Take(request.ItemCount)
                .Include(x => x.Cast)
            .ToListAsync(cancellationToken);

        return new PagedResponse<IEnumerable<TvShow>>(tvShows, totalCount);
    }

}
