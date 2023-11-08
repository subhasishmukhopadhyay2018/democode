using System.Text.Json.Serialization;
namespace AzureOpenAIWithRestAPI.Models.Output
{
    public class ChatCompletionResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("reated")]
        public int Created { get; set; }

        [JsonPropertyName("object")]
        public string? ObjectName { get; set; }

        [JsonPropertyName("choices")]
        public List<ChoicesOpenAI>? ChoicesOpenAI { get; set; }
    }
}