using System.Text.Json.Serialization;

namespace AzureOpenAIWithRestAPI.Models.Output
{
    public class MessageOutout
    {
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("end_turn")]
        public bool End_turn { get; set; }
    }
}