using System.Text.Json.Serialization;

public class DataSources
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("parameters")]
    public Parameters? Parameters { get; set; }
}
