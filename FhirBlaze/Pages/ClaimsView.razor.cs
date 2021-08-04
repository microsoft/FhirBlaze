using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Graph;

namespace FhirBlaze.Pages
{
    [Authorize()]
    public partial class ClaimsView
    {
        [Inject]
        IAccessTokenProvider TokenProvider { get; set; }

        [Inject]
        AuthenticationStateProvider context { get; set; }

        [Inject]
        GraphServiceClient graphClient { get; set; }

        public string AccessToken { get; set; }

        public string Name { get; set; }

        public string GraphQLName { get; set; }

        public string Errors { get; set; }

        public IEnumerable<Claim> Claims { get; set; }

        public User User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var res = context.GetAuthenticationStateAsync().Result;
            if (res.User.Identity.IsAuthenticated)
            {
                Claims = res.User.Claims;
                Name = res.User.Identity.Name;

                User = await graphClient.Me.Request().GetAsync();
            }
            else
            {
                Claims = null;
                Name = "No";
            }
            var accessTokenResult = await TokenProvider.RequestAccessToken();
            AccessToken = string.Empty;

            if (accessTokenResult.TryGetToken(out var token))
            {
                AccessToken = token.Value;
            }

        }

    }
}
