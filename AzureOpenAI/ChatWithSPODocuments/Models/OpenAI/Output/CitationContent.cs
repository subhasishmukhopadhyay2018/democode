using System.Text.Json.Serialization;

namespace AzureOpenAIWithRestAPI.Models.Output
{
    public class CitationContent
    {
        [JsonPropertyName("citations")]
        public List<Citation>? Citations { get; set; }

        [JsonPropertyName("intent")]
        public string? Intent { get; set; }
    }
}