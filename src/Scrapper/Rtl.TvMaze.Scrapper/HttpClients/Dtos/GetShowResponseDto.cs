using System.Text.Json.Serialization;

namespace Rtl.TvMaze.Scrapper.HttpClients.Dtos;

public class GetShowResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
