using Rtl.TvMaze.Scrapper.HttpClients.Dtos;

namespace Rtl.TvMaze.Scrapper.HttpClients;

public interface ITvMazeHttpClient
{
    Task<GetShowDetailResponseDto> GetShowDetailAsync(int showId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GetShowResponseDto>?> GetShowsAsync(int page = 0, CancellationToken cancellationToken = default);
}