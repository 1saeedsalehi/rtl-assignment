using Rtl.MazeScrapper.Application.HttpClients.Dtos;

namespace Rtl.MazeScrapper.Application.HttpClients;

public interface ITvMazeHttpClient
{
    Task<GetShowDetailResponseDto> GetShowDetailAsync(int showId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GetShowResponseDto>?> GetShowsAsync(int page = 0, CancellationToken cancellationToken = default);
}