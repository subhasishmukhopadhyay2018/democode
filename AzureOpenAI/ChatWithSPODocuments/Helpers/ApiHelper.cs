namespace Helpers.API
{
    using System.Net.Http.Headers;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// API helper interface.
    /// </summary>
    public class ApiHelper : IApiHelper
    {
        /// <summary>
        /// The HttpClient Factory
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;
        const string requestHeaderAuthorizationType = "Bearer";
        const string jsonContentType = "application/json";
        const double httpClientTimeout = 5;

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public ApiHelper(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Execute Get API.
        /// </summary>
        /// <typeparam name="T">Generic Parameter.</typeparam>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="accessToken">The access token.</param>
        /// <returns>The JSON object.</returns>
        public async Task<object> ExecuteGetAsync<T>(string requestUrl, string accessToken)
            where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var authHeader = new AuthenticationHeaderValue(requestHeaderAuthorizationType, accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(jsonContentType));
            request.Headers.Authorization = authHeader;

            HttpClient client = this.httpClientFactory.CreateClient();

            string responseContent;
            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    responseContent = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<T>(responseContent);
                    return jsonObject;
                }
                else
                {
                    responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseContent);
                }
            }
        }

        /// <summary>
        /// Retrieve results from Graph Endpoints.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="accessToken">The access token.</param>
        /// <returns>The JSON object.</returns>
        public async Task<HttpContent> DownloadFileAsync(string requestUrl, string accessToken)
        {
            using (HttpClient httpClient = this.httpClientFactory.CreateClient())
            {
                if (httpClient.Timeout < TimeSpan.FromMinutes(httpClientTimeout))
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(httpClientTimeout);
                }
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                // Make the HTTP GET request to download the file content
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                return response.Content;
            }
        }
    }
}
