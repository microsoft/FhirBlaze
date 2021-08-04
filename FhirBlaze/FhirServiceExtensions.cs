using System;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using FhirBlaze.SharedComponents.Services;
using FhirBlaze.SharedComponents;
using Microsoft.Authentication.WebAssembly.Msal.Models;

internal static class FhirServiceExtensions
{
    public static IServiceCollection AddFhirService(
        this IServiceCollection services, FhirDataConnection connection)
    {
        services.Configure<RemoteAuthenticationOptions<MsalProviderOptions>>(
            options =>
            {
                options.ProviderOptions.AdditionalScopesToConsent.Add(connection.Scope);
            });

        services.AddHttpClient<IFhirService, FhirService>(s => s.BaseAddress = new Uri(connection.FhirServerUri))
                .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
                    .ConfigureHandler(
                        authorizedUrls: new[] { connection.FhirServerUri },
                        scopes: new[] { connection.Scope }));

        return services;
    }
   
}