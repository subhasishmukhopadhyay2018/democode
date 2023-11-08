using Azure.AI.OpenAI;
using AzureOpenAIWithRestAPI.Models.Input;
using AzureOpenAIWithRestAPI.Models.Output;
using System.Text;
using System.Text.Json;

namespace ChatWithSPODocuments.Helpers
{
    public interface IOpenAIHelper
    {
        OpenAIClient GetOpenAIClient();
        Task GetOpenAIResponseAsync(string prompt);
    }
}