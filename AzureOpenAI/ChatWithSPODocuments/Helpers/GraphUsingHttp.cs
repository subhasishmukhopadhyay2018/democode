using ChatWithSPODocuments.Models;
using Helpers.API;

namespace ChatWithSPODocuments.Helpers
{

    public class GraphUsingHttp
    {
        private readonly IApiHelper apiHelper;
        private readonly AppConfig appConfig;
        public GraphUsingHttp(AppConfig appConfig, IApiHelper apiHelper)
        {
            this.appConfig = appConfig;
            this.apiHelper = apiHelper;
        }

        public async Task<dynamic?> GetSPOFilesAsync(string siteId, string authToken)
        {
            try
            {
                string spoGraphUrl = $"https://graph.microsoft.com/v1.0/sites/{siteId}/drive/root/children";
                dynamic getDocumentPermissionsObject = await apiHelper.ExecuteGetAsync<GraphResponse>(spoGraphUrl, authToken);
                if (getDocumentPermissionsObject != null)
                {
                    if(getDocumentPermissionsObject.value != null)
                    {
                        return getDocumentPermissionsObject.value;
                    }
                }
                return null;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
