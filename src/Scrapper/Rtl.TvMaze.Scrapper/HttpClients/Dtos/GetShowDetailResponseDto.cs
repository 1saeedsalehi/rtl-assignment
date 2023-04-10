using System.Text.Json.Serialization;

namespace Rtl.TvMaze.Scrapper.HttpClients.Dtos;

public class GetShowDetailResponseDto
{

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("_embedded")]
    public Embedded Embedded { get; set; }

}

public class Embedded
{
    [JsonPropertyName("cast")]
    public List<Cast> Cast { get; set; }
}
public class Cast
{
    [JsonPropertyName("person")]
    public Person? Person { get; set; }
}

public class Person
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("birthday")]
    public string? Birthday { get; set; }
}