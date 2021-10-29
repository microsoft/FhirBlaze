using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FhirBlaze.SharedComponents;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using FhirBlaze.Graph;
using System.Net.Http;
using FhirBlaze.SharedComponents.SMART;
using Blazored.Modal;

namespace FhirBlaze
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://graph.microsoft.com") });

            builder.Services.AddMsalAuthentication<RemoteAuthenticationState, RemoteUserAccount>(options =>
            {
                var scopes = builder.Configuration.GetValue<string>("GraphScopes");
                if (string.IsNullOrEmpty(scopes))
                {
                    Console.WriteLine("WARNING: No permission scopes were found in the GraphScopes app setting. Using default User.Read.");
                    scopes = "User.Read";
                }

                foreach (var scope in scopes.Split(';'))
                {
                    Console.WriteLine($"Adding {scope} to requested permissions");
                    options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
                }

                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            })
            .AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, GraphUserAccountFactory>();

            builder.Services.AddScoped<GraphClientFactory>();

            builder.Services.AddFhirService(() => 
            {
                var fhir = new FhirDataConnection();
                builder.Configuration.Bind("FhirConnection", fhir);
                return fhir;
            });

            builder.Services.AddBlazoredModal();

            builder.Services.AddScoped<SmartLauncher>(o => 
            {
                var launcher = new SmartLauncher(builder.Configuration["ChestistApp:LaunchUrl"], builder.Configuration["FhirConnection:FhirServerUri"]);
                return launcher;
            });

            await builder.Build().RunAsync();
        }
    }
}