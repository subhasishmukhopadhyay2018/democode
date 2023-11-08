using Azure;
using Azure.AI.OpenAI;
using AzureOpenAIWithRestAPI.Models.Input;
using AzureOpenAIWithRestAPI.Models.Output;
using ChatWithSPODocuments.Models;
using System.Text;
using System.Text.Json;

namespace ChatWithSPODocuments.Helpers
{
    internal class OpenAIHelper : IOpenAIHelper
    {
        private readonly string OpenAIUrl;
        private readonly string OpenAIKey;
        private readonly string DeploymentName;
        private readonly string ChatCompletionUrlSchema;
        private readonly string SearchIndexName;
        private readonly string SearchEndPoint;
        private readonly string ACSSearchKey;
        private readonly OpenAIClient OpenAIClientObject;
        private readonly string SemanticSearchConfigName;
        private readonly string EmbeddingEndpoint;
        private readonly string EmbeddingKey;
        private readonly string EmbeddingDeploymentName;
        private readonly string OpenAIQueryType;

        public OpenAIHelper(AppConfig appConfig)
        {
            OpenAIUrl = appConfig.OpenAIUrl;
            OpenAIKey = appConfig.OpenAIKey;
            DeploymentName = appConfig.DeploymentName;
            ChatCompletionUrlSchema = appConfig.ChatCompletionUrlSchema;
            SearchEndPoint = appConfig.ACSSearchEndpoint;
            SearchIndexName = appConfig.ACSSearchIndex;
            ACSSearchKey = appConfig.ACSSearchKey;
            OpenAIClientObject = new OpenAIClient(new Uri(appConfig.OpenAIUrl), new AzureKeyCredential(appConfig.OpenAIKey));
            SemanticSearchConfigName = appConfig.SemanticSearchConfigName;
            EmbeddingEndpoint = appConfig.EmbeddingEndpoint;
            EmbeddingKey = appConfig.OpenAIKey;
            EmbeddingDeploymentName = appConfig.EmbeddingModelName;
            OpenAIQueryType = appConfig.OpenAIQueryType;
        }

        public OpenAIClient GetOpenAIClient()
        {
            return OpenAIClientObject;
        }

        public static string GetFakeGuids(int? count = 200)
        {
            // create a list of fake guids
            List<string> fakeGuids = new List<string>();
            for (int i = 0; i < count; i++)
            {
                fakeGuids.Add(Guid.NewGuid().ToString());
            }
            // convert list to string with comma separated values
            string fakeGuidsString = string.Join(",", fakeGuids);
            return fakeGuidsString;
        }

        public async Task GetOpenAIResponseAsync(string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                string chatCompletionUrl = string.Format(ChatCompletionUrlSchema, OpenAIUrl, DeploymentName);
                string embedEndPointUrl = String.Format(EmbeddingEndpoint, OpenAIUrl, EmbeddingDeploymentName);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.OpenAIKey}");

                var request = new HttpRequestMessage(HttpMethod.Post, chatCompletionUrl);
                request.Headers.Add("api-key", this.OpenAIKey);
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                ChatPostMessage chatPostMessage = new ChatPostMessage()
                {
                    Messages = new List<Message>()
                    {
                        new Message()
                        {
                            Role = "user",
                            Content = prompt
                        }
                    },
                    Temperature = 0.2f,
                    DataSources = new List<DataSources>()
                    {
                        new DataSources()
                        {
                            Type = "AzureCognitiveSearch",

                            Parameters = new Parameters()
                            {
                                Endpoint = $"{SearchEndPoint}",
                                Key = $"{ACSSearchKey}",
                                IndexName = $"{SearchIndexName}",
                                QueryType = OpenAIQueryType,
                                TopNDocuments = 5,
                                FieldsMapping = new ()
                                {
                                    TitleField = "id" ,
                                    UrlField = "url",
                                    FilepathField = "chunk_id",
                                    ContentFields = new List<string> { "url","filepath", "content" },
                                    VectorFields = new List<string> { "titleVector", "contentVector" },
                                    ContentFieldsSeparator = "#########\n"
                                },
                                SemanticConfiguration = SemanticSearchConfigName,
                                Filter = $"AAD_GROUPS/any(g:search.in(g, '{GetFakeGuids(300)},SampleGroup1,SampleGroup2'))",
                                EmbeddingDeploymentName = EmbeddingDeploymentName
                            }
                        }
                    }
                };

                string jsonStr = JsonSerializer.Serialize(chatPostMessage);
                // replace \u0027 with ' in jsonStr
                jsonStr = jsonStr.Replace("\\u0027", "'");
                request.Content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.SendAsync(request);

                string responseBody = await response.Content.ReadAsStringAsync();
                ChatCompletionResponse? completionResponse = JsonSerializer.Deserialize<ChatCompletionResponse>(responseBody);
                if ((completionResponse == null) || (completionResponse?.ChoicesOpenAI == null))
                {
                    Console.WriteLine("completionResponse is null");
                    return;
                }
                Console.WriteLine("===========================================");
                try
                {
                    string answer = answer = completionResponse?.ChoicesOpenAI[0]?.Messages?.Find(x => x.Role == "assistant")?.Content ?? string.Empty;
                    Console.WriteLine($"Answer: {answer}");

                    foreach (ChoicesOpenAI choice in completionResponse?.ChoicesOpenAI)
                    {
                        if (choice.Messages == null)
                        {
                            Console.WriteLine("choice.messages is null");
                            continue;
                        }
                        foreach (var message in choice.Messages)
                        {
                            if ((message !=null) &&  (message.Role.Equals("tool", StringComparison.OrdinalIgnoreCase)))
                            {
                                CitationContent? citations = JsonSerializer.Deserialize<CitationContent>(message.Content);
                                if (citations == null)
                                {
                                    Console.WriteLine("citations is null");
                                    continue;
                                }
                                short i = 0;
                                foreach (var citation in citations.Citations)
                                {
                                    Console.WriteLine($"=== Citation Details [Doc{++i}]===");
                                    Console.WriteLine($"File: {citation.FilePath}");
                                    Console.WriteLine($"Title: {citation.Title}");
                                    Console.WriteLine($"URL: {citation.Url}");
                                    Console.WriteLine($"Content: {citation.Content}");
                                    Console.WriteLine("================================================");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine($"Exception: {ex.Message}"); }

                Console.WriteLine("===");
            }
        }
    }
}
