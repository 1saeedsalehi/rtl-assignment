using Rtl.TvMaze.Infrastructure;
using Rtl.TvMaze.Scrapper.HttpClients.Dtos;
using System.Net.Http.Json;

namespace Rtl.TvMaze.Scrapper.HttpClients;

public class TvMazeHttpClient : ITvMazeHttpClient
{
    private readonly HttpClient _httpClient;

    public TvMazeHttpClient(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient(Constants.TvMazeHttpClient);// ?? throw new ArgumentNullException(nameof(httpClient));

    }

    public async Task<IEnumerable<GetShowResponseDto>?> GetShowsAsync(int page = 0, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<GetShowResponseDto>>($"shows?page={page}", cancellationToken);

            return response;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<GetShowDetailResponseDto?> GetShowDetailAsync(int showId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<GetShowDetailResponseDto>($"shows/{showId}?embed=cast", cancellationToken);
        return response;

    }
}
