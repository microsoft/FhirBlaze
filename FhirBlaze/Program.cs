using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FhirBlaze.SharedComponents;
using FhirBlaze.SharedComponents.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace FhirBlaze
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            var fhir = new FhirDataConnection
            {
                Scope = "https://hlsfhirpower.azurehealthcareapis.com/user_impersonation",
                FhirServerUri = "https://hlsfhirpower.azurehealthcareapis.com/metadata",
                Authority = "https://login.microsoftonline.com"
            };
           // builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add(fhir.Scope);

            });
            
            builder.Services.AddHttpClient<IFHIRBlazeServices, FHIRBlazeServices>
    (s =>
        s.BaseAddress = new Uri(fhir.FhirServerUri))
        .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
        .ConfigureHandler(
                authorizedUrls: new[] { fhir.FhirServerUri },
                scopes: new[] { fhir.Scope }));


            await builder.Build().RunAsync();
        }
    }
}
