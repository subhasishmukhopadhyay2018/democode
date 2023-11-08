namespace Helpers.API
{
    /// <summary>
    /// API helper.
    /// </summary>
    public interface IApiHelper
    {
        /// <summary>
        /// Execute Get API.
        /// </summary>
        /// <typeparam name="T">Generic Parameter.</typeparam>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="accessToken">The access token.</param>
        /// <returns>The JSON object.</returns>
        Task<object> ExecuteGetAsync<T>(string requestUrl, string accessToken) where T : class;

        /// <summary>
        /// Download file asycn.
        /// </summary>
        /// <typeparam name="T">Generic Parameter.</typeparam>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="requestBodyObject">The request body object.</param>
        /// <param name="accessToken">The access token.</param>
        /// <returns>The JSON object.</returns>
        Task<HttpContent> DownloadFileAsync(string requestUrl, string accessToken);

        
    }
}
