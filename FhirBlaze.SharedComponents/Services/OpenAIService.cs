using FhirBlaze.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.SharedComponents.Services;

public class OpenAIService
{
    private HttpClient _httpClient;
    private readonly string _model;
    private readonly AiService _aiService;

    public OpenAIService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _aiService = bool.Parse(configuration.GetSection("OpenAi")["UseAzure"]) ? AiService.AzureOpenAi : AiService.OpenAi;
        _model = configuration.GetSection("OpenAi")["Model"];
    }

    public async Task<string> GetFhirQueryFromNaturalLanguage(string prompt)
    {
        var response = await _httpClient.PostAsync("/api/GetFhirQuery", new StringContent(JsonConvert.SerializeObject(new FhirQueryRequest
        {
            Prompt = prompt,
            Model = _model,
            Service = _aiService
        }), Encoding.Default, "application/json"));

        var respModel = JsonConvert.DeserializeObject<CompletionResponse>(await response.Content.ReadAsStringAsync());

        return respModel.Choices.FirstOrDefault()?.Text?.Trim();
    }
}

