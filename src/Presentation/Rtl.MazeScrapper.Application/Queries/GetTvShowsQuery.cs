using MediatR;
using Microsoft.EntityFrameworkCore;
using Rtl.TvMaze.Domain.Entities;
using Rtl.TvMaze.Persistence;

namespace Rtl.TvMaze.Application.Queries;

public class GetTvShowsQuery : IRequest<IEnumerable<TvShow>>
{
    public int Page { get; set; } = 0;
    public int ItemCount { get; set; } = 10;

}

public class GetTvShowsQueryHandler : IRequestHandler<GetTvShowsQuery, IEnumerable<TvShow>>
{
    private readonly TvMazeDbContext _dbContext;

    public GetTvShowsQueryHandler(TvMazeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TvShow>> Handle(GetTvShowsQuery request,
        CancellationToken cancellationToken)
    {
        var tvShows = await _dbContext.TvShows
            .AsNoTracking()
            .Skip(request.Page * request.ItemCount)
            .Take(request.ItemCount)
                .Include(x => x.Cast)
            .ToListAsync(cancellationToken);

        return tvShows;
    }

}
