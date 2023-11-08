using Microsoft.Graph.Models.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWithSPODocuments.Models.ACS
{
    public class DocumentIndex
    {
        // create class properties based on the structure of the JSON response
        public string id { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public string filepath { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string chunk_id { get; set; } = string.Empty;
        public string last_updated { get; set; } = string.Empty;
        public string[] AAD_GROUPS { get; set; } = new string[] { };
        public float[] titleVector { get; set; } = new float[] { };
        public float[] contentVector { get; set; } = new float[] { };
    }
}
