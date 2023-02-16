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
    private static readonly string AZURE_OPENAI_API_KEY = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
    private static readonly string AZURE_OPENAI_INSTANCE = Environment.GetEnvironmentVariable("AZURE_OPENAI_INSTANCE");
    private static readonly string OPENAI_API_KEY = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
    private static readonly string OPENAI_INSTANCE = Environment.GetEnvironmentVariable("OPENAI_API_INSTANCE");
    private static readonly string PROMPT_PREFIX = "Generate a FHIR r4 query for ";

    [FunctionName("GetFhirQuery")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(requestBody))
            return new BadRequestObjectResult("Invalid Request");

        var fhirQueryRequest = JsonConvert.DeserializeObject<FhirQueryRequest>(requestBody);

        var request = new CompletionRequest()
        {
            Model = fhirQueryRequest.Model,
            Prompt = $"{PROMPT_PREFIX}{fhirQueryRequest.Prompt}",
            Temperature = 1,
            TopP = 1,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            BestOf = 1,
            N = 1,
            MaxTokens = 100,
            Stop = null
        };

        using var client = new HttpClient();

        HttpResponseMessage response;
        CompletionResponse result;

        switch(fhirQueryRequest.Service)
        {
            case AiService.AzureOpenAi:
                client.BaseAddress = new Uri($"https://{AZURE_OPENAI_INSTANCE}");
                client.DefaultRequestHeaders.Add("API_KEY", AZURE_OPENAI_API_KEY);
                response = await client.PostAsJsonAsync("/openai/deployments/text_davinci_002/completions?api_version=2022_12_01", request);
                result = JsonConvert.DeserializeObject<CompletionResponse>(await response.Content.ReadAsStringAsync());
                return new OkObjectResult(result);
            case AiService.OpenAi:
                client.BaseAddress = new Uri($"https://{OPENAI_INSTANCE}");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {OPENAI_API_KEY}");
                response = await client.PostAsJsonAsync("/v1/completions", request);
                result = JsonConvert.DeserializeObject<CompletionResponse>(await response.Content.ReadAsStringAsync());
                return new OkObjectResult(result);
            default:
                return new BadRequestObjectResult("Invalid AI Service");
        }


    }
}
