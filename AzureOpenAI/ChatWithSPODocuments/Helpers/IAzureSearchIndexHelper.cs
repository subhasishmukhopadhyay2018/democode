using Azure.AI.OpenAI;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using ChatWithSPODocuments.Models.ACS;
using ChatWithSPODocuments.Models;

namespace ChatWithSPODocuments.Helpers
{
    internal interface IAzureSearchIndexHelper
    {
        Task InsertToSearchIndexStoreAsync(List<DocumentIndex> results);
        Task DeleteFromSearchIndexStoreAsync(string filter);
        void CreateOrUpdateIndex();
        Task<List<DocumentIndex>> GenerateDocumentIndexDateAsync(OpenAIClient openAIClient, List<string> paragraphs, Value itemValue);
    }
}