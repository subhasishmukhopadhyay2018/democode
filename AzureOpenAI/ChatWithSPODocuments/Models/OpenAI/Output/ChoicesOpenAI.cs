using System.Text.Json.Serialization;
namespace AzureOpenAIWithRestAPI.Models.Output
{
    public class ChoicesOpenAI
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("messages")]
        public List<MessageOutout>? Messages { get; set; }
    }
}
