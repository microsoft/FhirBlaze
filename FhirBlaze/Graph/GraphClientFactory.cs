using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace FhirBlaze.Graph
{
    public class GraphClientFactory
    {
        private readonly IAccessTokenProviderAccessor accessor;
        private readonly HttpClient httpClient;
        private readonly ILogger<GraphClientFactory> logger;
        private GraphServiceClient graphClient;

        public GraphClientFactory(IAccessTokenProviderAccessor accessor,
            HttpClient httpClient,
            ILogger<GraphClientFactory> logger)
        {
            this.accessor = accessor;
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public GraphServiceClient GetAuthenticatedClient()
        {
            // Use the existing one if it's there
            if (graphClient == null)
            {
                // Create a GraphServiceClient using a scoped
                // HttpClient
                graphClient = new GraphServiceClient(httpClient);
                // Configure the auth provider
                graphClient.AuthenticationProvider =
                    new BlazorAuthProvider(accessor);
            }

            return graphClient;
        }
    }
}
