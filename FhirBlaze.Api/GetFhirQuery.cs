using FhirBlaze.Api.Models;
using FhirBlaze.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace FhirBlaze.Api;

public static class GetFhirQuery
{
    private static readonly string API_KEY = Environment.GetEnvironmentVariable("OPENAI-API-KEY");
    private static readonly string OPENAI_INSTANCE = Environment.GetEnvironmentVariable("OPENAI-INSTANCE");
    private static readonly string PROMPT_PREFIX = "Generate a fhir query for ";

    [FunctionName("GetFhirQuery")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        dynamic data = JsonConvert.DeserializeObject(requestBody);

        var prompt = data?.prompt ?? string.Empty;

        if (string.IsNullOrEmpty(prompt))
            return new BadRequestObjectResult("Invalid Request");
        
        var request = new CompletionRequest()
        {
            Prompt = $"{PROMPT_PREFIX}{prompt}",
            Temperature = 1,
            TopP = 0.5m,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            BestOf = 1,
            MaxTokens = 100,
            Stop = null
        };

        using var client = new HttpClient();

        client.BaseAddress = new Uri($"https://{OPENAI_INSTANCE}");
        client.DefaultRequestHeaders.Add("API-KEY", API_KEY);

        var response = await client.PostAsJsonAsync("/openai/deployments/text-davinci-002/completions?api-version=2022-12-01", request);

        var result = JsonConvert.DeserializeObject<CompletionResponse>(await response.Content.ReadAsStringAsync());

        return new OkObjectResult(result);
    }
}
