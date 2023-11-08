using ChatWithSPODocuments.Models.OpenAI.Input;
using System.Text.Json.Serialization;

public class Parameters
{
    [JsonPropertyName("endpoint")]
    public string Endpoint { get; set; }

    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("indexName")]
    public string IndexName { get; set; }

    [JsonPropertyName("filter")]
    public string? Filter { get; set; }

    [JsonPropertyName("queryType")]
    public string? QueryType { get; set; }

    [JsonPropertyName("semanticConfiguration")]

    public string? SemanticConfiguration { get; set; }

    [JsonPropertyName("topNDocuments")]
    public int? TopNDocuments { get; set; }

    [JsonPropertyName("fieldsMapping")]
    public FieldsMapping? FieldsMapping { get; set; }

    [JsonPropertyName("embeddingEndpoint")]
    public string? EmbeddingEndpoint { get; set; } = null;

    [JsonPropertyName("embeddingKey")]
    public string? EmbeddingKey { get; set; } = null;

    [JsonPropertyName("embeddingDeploymentName")]
    public string? EmbeddingDeploymentName { get; set; } = null;
}
