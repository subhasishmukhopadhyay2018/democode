using System.Text.Json.Serialization;
namespace AzureOpenAIWithRestAPI.Models.Output
{
    public class Citation
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("filepath")]
        public string FilePath { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        [JsonPropertyName("chunk_id")]
        public string ChunkId { get; set; }
    }
}