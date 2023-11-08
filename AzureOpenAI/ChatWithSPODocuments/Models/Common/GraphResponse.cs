using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatWithSPODocuments.Models
{
    // create classes to deserialize the JSON response
    public class GraphResponse
    {
        public string? odatacontext { get; set; }
        public string? microsoftgraphtips { get; set; }
        public List<Value>? value { get; set; }
    }

    // create classes to deserialize the JSON response for Value
    public class Value
    {
        [JsonProperty(PropertyName = "@microsoft.graph.downloadUrl")]
        public string MicrosoftGraphdownloadUrl { get; set; } = string.Empty;
        public string? createdDateTime { get; set; }
        public string? eTag { get; set; }
        public string? id { get; set; }
        public string? lastModifiedDateTime { get; set; }
        public string? name { get; set; }
        public string? webUrl { get; set; }
        public string? cTag { get; set; }
        public int? size { get; set; }
        public int? chunk_id { get; set; }
    }
    
}
