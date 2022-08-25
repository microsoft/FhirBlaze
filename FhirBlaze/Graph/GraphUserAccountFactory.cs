using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;

using System.Security.Claims;
using System.Threading.Tasks;

namespace FhirBlaze.Graph
{
    // Extends the AccountClaimsPrincipalFactory that builds
    // a user identity from the identity token.
    // This class adds additional claims to the user's ClaimPrincipal
    // that hold values from Microsoft Graph
    public class GraphUserAccountFactory
        : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        private readonly IAccessTokenProviderAccessor accessor;
        private readonly ILogger<GraphUserAccountFactory> logger;
        private readonly GraphClientFactory clientFactory;

        public GraphUserAccountFactory(IAccessTokenProviderAccessor accessor,
            GraphClientFactory clientFactory,
            ILogger<GraphUserAccountFactory> logger)
        : base(accessor)
        {
            this.accessor = accessor;
            this.clientFactory = clientFactory;
            this.logger = logger;
        }

        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(
            RemoteUserAccount account,
            RemoteAuthenticationUserOptions options)
        {
            // Create the base user
            var initialUser = await base.CreateUserAsync(account, options);

            // If authenticated, we can call Microsoft Graph
            if (initialUser.Identity is not { IsAuthenticated: true }) return initialUser;
            
            try
            {
                // Add additional info from Graph to the identity
                await AddGraphInfoToClaims(initialUser);
            }
            catch (AccessTokenNotAvailableException exception)
            {
                logger.LogError("Graph API access token failure: {ExceptionMessage}", exception.Message);
            }
            catch (Microsoft.Graph.ServiceException exception)
            {
                logger.LogError("Graph API error: {ExceptionMessage}", exception.Message);
                logger.LogError("Response body: {ExceptionRawResponseBody}", exception.RawResponseBody);
            }

            return initialUser;
        }

        private async Task AddGraphInfoToClaims(ClaimsPrincipal claimsPrincipal)
        {
            var serviceClient = clientFactory.GetAuthenticatedClient();

            // Get user profile including mailbox settings
            // GET /me?$select=displayName,mail,mailboxSettings,userPrincipalName
            var user = await serviceClient.Me
                .Request()
                // Request only the properties used to
                // set claims
                .Select(u => new
                {
                    u.DisplayName,
                    u.Mail,
                    u.UserPrincipalName
                })
                .GetAsync();

            logger.LogInformation($"Got user: {user.DisplayName}");

            claimsPrincipal.AddUserGraphInfo(user);

            // Get user's photo
            // GET /me/photos/48x48/$value
            var photo = await serviceClient.Me
                .Photos["48x48"]  // Smallest standard size
                .Content
                .Request()
                .GetAsync();

            claimsPrincipal.AddUserGraphPhoto(photo);
        }
    }
}
