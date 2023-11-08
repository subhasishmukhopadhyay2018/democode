using ChatWithSPODocuments.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System.Text;
using Helpers.API;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using ChatWithSPODocuments.Models.ACS;
using Microsoft.SemanticKernel.Text;
using ChatWithSPODocuments.Models;
using Microsoft.IdentityModel.Tokens;

class Program
{
    private static IConfiguration? configuration;

    static async Task Main(string[] args)
    {
        configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appSettings.json", false)
        .AddJsonFile("appSettings.dev.json", false)
        .Build();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(Program.configuration)
            .BuildServiceProvider();
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        AppConfig? appConfig = config.GetSection("AppConfig").Get<AppConfig>();

        Console.WriteLine("Connecting to GraphAI for SPO Document Library");
        if (appConfig == null)
        {
            throw new Exception("Configuration missing!");
        }

        serviceProvider = new ServiceCollection()
            .AddTransient<IApiHelper, ApiHelper>()
            .AddTransient<GraphUsingHttp>()
            .AddTransient<IOpenAIHelper, OpenAIHelper>(_ => new(appConfig))
            .AddTransient<IAzureSearchIndexHelper, AzureSearchIndexHelper>(_ => new(appConfig))
            .AddHttpClient()
            .BuildServiceProvider();

        try
        {
            IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            IApiHelper apiHelper = serviceProvider.GetRequiredService<IApiHelper>();
            IOpenAIHelper openAIHelper = serviceProvider.GetRequiredService<IOpenAIHelper>();
            GraphUsingHttp graphUsingHttp = new(appConfig, apiHelper);
            IAzureSearchIndexHelper azureSearchIndexHelper = serviceProvider.GetRequiredService<IAzureSearchIndexHelper>();

            var client = PublicClientApplicationBuilder.Create(appConfig.ClientId)
                .WithAuthority("https://login.microsoftonline.com/" + appConfig.TenantId, false)
                .WithRedirectUri("http://localhost")
                .Build();
            string ServerAppIdUri = $"https://graph.microsoft.com";

            Console.Write("Would you like to create or update index (y/n)? ");
            string indexChoice = Console.ReadLine()?.ToLower() ?? string.Empty;

            if (indexChoice.ToLower() == "y")
            {
                // Create the search index  
                azureSearchIndexHelper.CreateOrUpdateIndex();
            }
            Console.Write("Would you like to create or update index data (y/n)? ");
            string indexDataChoice = Console.ReadLine()?.ToLower() ?? string.Empty;

            if (indexDataChoice.ToLower() == "y")
            {
                string[] scopes = new string[] { "user.read", "files.read.all" };
                AuthenticationResult authenticationResult = client.AcquireTokenInteractive(scopes).ExecuteAsync().GetAwaiter().GetResult();
                var jsonContent = await graphUsingHttp.GetSPOFilesAsync(appConfig.SiteId, authenticationResult.AccessToken);
                if (jsonContent == null)
                {
                    Console.WriteLine("No files found in the SPO Document Library");
                    return;
                }
                foreach (var item in jsonContent)
                {
                    Value itemValue = (Value)item;
                    HttpContent fileContent = await apiHelper.DownloadFileAsync(itemValue.MicrosoftGraphdownloadUrl, authenticationResult.AccessToken);

                    Stream streamContent = await fileContent.ReadAsStreamAsync();
                    PdfReader reader = new PdfReader(streamContent);
                    StringBuilder textBuilder = new StringBuilder();

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        var pageText = PdfTextExtractor.GetTextFromPage(reader, i);
                        textBuilder.Append(pageText);
                    }
                    reader.Close();

                    List<string> paragraphs = TextChunker.SplitPlainTextParagraphs(TextChunker.SplitPlainTextLines(textBuilder.ToString(), 128), 1024, 50);
                    List<DocumentIndex> documentIndexes = await azureSearchIndexHelper.GenerateDocumentIndexDateAsync(openAIHelper.GetOpenAIClient(), paragraphs, itemValue);
                    await azureSearchIndexHelper.InsertToSearchIndexStoreAsync(documentIndexes);
                }
            }
            Console.Write("Please ask question? (e.g. Which date is celebrated as Independence day of India?)");
            string question = Console.ReadLine()?.ToLower() ?? string.Empty;

            while (!question.IsNullOrEmpty())
            {
                await openAIHelper.GetOpenAIResponseAsync(question);
                Console.Write("\n\n");
                Console.Write("Please ask question again? (e.g. Which date is celebrated as Independence day of India?)");
                question = Console.ReadLine()?.ToLower() ?? string.Empty;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
