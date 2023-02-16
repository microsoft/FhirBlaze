using FhirBlaze.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.SharedComponents.Services
{
    public class OpenAIService
    {
        private HttpClient _httpClient;

        public OpenAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetFhirQueryFromNaturalLanguage(string prompt, AiService service = AiService.AzureOpenAi)
        {
            var response = await _httpClient.PostAsync("/api/GetFhirQuery", new StringContent(JsonConvert.SerializeObject(new
            {
                prompt,
                service
            }), Encoding.Default, "application/json"));

            var respModel = JsonConvert.DeserializeObject<CompletionResponse>(await response.Content.ReadAsStringAsync());

            return respModel.Choices.FirstOrDefault()?.Text?.Trim();
        }
    }
}

