using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
            RemoteUserAccount account,
            RemoteAuthenticationUserOptions options)
        {
            // Create the base user
            var initialUser = await base.CreateUserAsync(account, options);

            // If authenticated, we can call Microsoft Graph
            if (initialUser.Identity.IsAuthenticated)
            {
                try
                {
                    // Add additional info from Graph to the identity
                    await AddGraphInfoToClaims(accessor, initialUser);
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    logger.LogError($"Graph API access token failure: {exception.Message}");
                }
                catch (Microsoft.Graph.ServiceException exception)
                {
                    logger.LogError($"Graph API error: {exception.Message}");
                    logger.LogError($"Response body: {exception.RawResponseBody}");
                }
            }

            return initialUser;
        }

        private async Task AddGraphInfoToClaims(
            IAccessTokenProviderAccessor accessor,
            ClaimsPrincipal claimsPrincipal)
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
            try
            {
                var photo = await serviceClient.Me
                    .Photos["48x48"]  // Smallest standard size
                    .Content
                    .Request()
                    .GetAsync();

                claimsPrincipal.AddUserGraphPhoto(photo);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to retrieve user image.");
            }
        }
    }
}
