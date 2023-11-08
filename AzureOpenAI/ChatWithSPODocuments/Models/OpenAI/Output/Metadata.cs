using System.Text.Json.Serialization;

namespace AzureOpenAIWithRestAPI.Models.Output
{
    public class Metadata
    {
        [JsonPropertyName("chunking")]
        public string? Chunking { get; set; }
    }
}