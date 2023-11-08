using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatWithSPODocuments.Models.OpenAI.Input
{
    public class FieldsMapping
    {
        [JsonPropertyName("titleField")]
        public string TitleField { get; set; }

        [JsonPropertyName("urlField")]
        public string UrlField { get; set; }

        [JsonPropertyName("filepathField")]
        public string FilepathField { get; set; }

        [JsonPropertyName("contentFields")]
        public List<string> ContentFields { get; set; }

        [JsonPropertyName("vectorFields")]
        public List<string> VectorFields { get; set; }

        [JsonPropertyName("contentFieldsSeparator")]
        public string ContentFieldsSeparator { get; set; }

    }
}
