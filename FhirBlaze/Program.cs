using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FhirBlaze.SharedComponents;
using Microsoft.Graph;

namespace FhirBlaze
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(fhir => new FhirDataConnection { FhirServerUri = "https://fhirserver.sample.com", Authority = "https://login.microsoftonline.com" });           

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            });

            builder.Services.AddGraphClient("https://graph.microsoft.com/User.Read");

            await builder.Build().RunAsync();
        }
    }
}
