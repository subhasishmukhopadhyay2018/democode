using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWithSPODocuments.Models
{
    public class AppConfig
    {
        public string ClientId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string GraphScope { get; set; } = string.Empty;
        public string SiteId { get; set; } = string.Empty;
        public string OpenAIUrl { get; set; } = string.Empty;
        public string OpenAIKey { get; set; } = string.Empty;
        public string DeploymentName { get; set; } = string.Empty;
        public string ChatCompletionUrlSchema { get; set; } = string.Empty;
        public string ACSSearchIndex { get; set; } = string.Empty;
        public string ACSSearchEndpoint { get; set; } = string.Empty;
        public string ACSSearchKey { get; set; } = string.Empty;
        public string EmbeddingModelName { get; set; } = string.Empty;
        public int ModelDimensions { get; set; }
        public string SemanticSearchConfigName { get; set; } = string.Empty;
        public string VectorSearchProfile { get; set; } = string.Empty;
        public string VectorSearchHnswConfig { get; set; } = string.Empty;
        public string EmbeddingEndpoint { get; set; } = string.Empty;
        public string ApiVersionEmbedding { get; set; } = string.Empty;
        public string OpenAIQueryType { get; set; } = string.Empty;
    }
}
