using System.Text.Json.Serialization;

namespace Rtl.MazeScrapper.Application.HttpClients.Dtos;

public class GetShowResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
