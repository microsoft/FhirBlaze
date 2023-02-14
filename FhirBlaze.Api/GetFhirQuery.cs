using FhirBlaze.Api.Models;
using FhirBlaze.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace FhirBlaze.Api
{
    public static class GetFhirQuery
    {
        static string API_KEY = Environment.GetEnvironmentVariable("OPENAI-API-KEY");
        static string OPENAI_INSTANCE = Environment.GetEnvironmentVariable("OPENAI-INSTANCE");
        static string PROMPT_PREFIX = "Generate a fhir query for ";

        [FunctionName("GetFhirQuery")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string prompt = string.Empty;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            prompt =  data?.prompt;

            if (!string.IsNullOrEmpty(prompt))
            {
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

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"https://{OPENAI_INSTANCE}");
                    client.DefaultRequestHeaders.Add("API-KEY", API_KEY);

                    var response = await client.PostAsJsonAsync("/openai/deployments/text-davinci-002/completions?api-version=2022-12-01", request);

                    var result = JsonConvert.DeserializeObject<CompletionResponse>(await response.Content.ReadAsStringAsync());

                    return new OkObjectResult(result);
                }
            }

            return new BadRequestObjectResult("Invalid Request");
        }
    }
}

