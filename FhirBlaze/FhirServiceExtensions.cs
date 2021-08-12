using System;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using FhirBlaze.SharedComponents.Services;
using FhirBlaze.SharedComponents;
using Microsoft.Authentication.WebAssembly.Msal.Models;
using System.Net.Http;
using Hl7.Fhir.Rest;

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


        services.AddScoped<FhirClient>(o =>
        {
            var settings = new FhirClientSettings
            {
                PreferredFormat = ResourceFormat.Json,
                PreferredReturn = Prefer.ReturnMinimal
            };
            var handler = o.GetRequiredService<AuthorizationMessageHandler>()
                .ConfigureHandler(
                    authorizedUrls: new[] { fhirData.FhirServerUri },
                    scopes: new[] { fhirData.Scope }                    
                );
            handler.InnerHandler = new HttpClientHandler();

            return new FhirClient(fhirData.FhirServerUri, settings, handler);
        });

        services.AddScoped<IFhirService, FirelyService>();

        return services;
    }
   
}