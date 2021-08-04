using System;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using FhirBlaze.SharedComponents.Services;
using FhirBlaze.SharedComponents;
using Microsoft.Authentication.WebAssembly.Msal.Models;

internal static class FhirServiceExtensions
{
    public static IServiceCollection AddFhirService(
        this IServiceCollection services, Func<FhirDataConnection> connection)
    {
        var fhirData = connection.Invoke();        

        services.Configure<RemoteAuthenticationOptions<MsalProviderOptions>>(
            options =>
            {                
                options.ProviderOptions.AdditionalScopesToConsent.Add(fhirData.Scope);
            });

        services.AddHttpClient<IFhirService, FhirService>(s => s.BaseAddress = new Uri(fhirData.FhirServerUri))
                .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                    .ConfigureHandler(
                        authorizedUrls: new[] { fhirData.FhirServerUri },
                        scopes: new[] { fhirData.Scope }));

        return services;
    }
   
}