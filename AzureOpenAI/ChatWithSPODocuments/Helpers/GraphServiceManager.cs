using Azure.Identity;
using ChatWithSPODocuments.Models;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Kiota.Abstractions;

namespace PermissionMigrationFromGDriveToSPO.Managers
{
    public class GraphServiceManager
    {
        private readonly GraphConfig graphConfig;
        public GraphServiceManager(AppConfig appConfig)
        {
            graphConfig = new GraphConfig();
            graphConfig.ClientId = appConfig.ClientId;
            graphConfig.TenantId = appConfig.TenantId;
        }

        
        public GraphServiceClient GetGraphServiceClientInteractive()
        {
            var scopes = new[] { "User.Read", "Files.Read.All" };
            //var scopes = new[] { "https://graph.microsoft.com/.default" };

            var clientId = graphConfig.ClientId;
            var tenantId = graphConfig.TenantId;

            // using Azure.Identity;
            var options = new InteractiveBrowserCredentialOptions
            {
                TenantId = tenantId,
                ClientId = clientId,
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                RedirectUri = new Uri("http://localhost"),
            };

            var interactiveCredential = new InteractiveBrowserCredential(options);

            var graphClient = new GraphServiceClient(interactiveCredential, scopes);

            return graphClient;
        }

        public async Task<List<string>> GetAttachmentsFromSPOLibrary(GraphServiceClient graphClient, string siteId)
        {
            List<string> driveItems = new List<string>();
            var drive = await graphClient.Sites[siteId].Drive.GetAsync();
            var v = await graphClient.Drives[drive?.Id].Items.GetAsync();
            var file = drive.Items[0].File?.ToString();
            drive?.Root?.Children?.ForEach(file =>
                {
                    driveItems.Add(file.Name);
                    Console.WriteLine(file.Name);
                }
            );
            return driveItems;
        }
    }
}
