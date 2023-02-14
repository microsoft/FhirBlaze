using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FhirBlaze.SharedComponents.Services
{
    public class OpenAIService
    {
        private HttpClient _httpClient;

        public OpenAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task GetFhirQueryFromNaturalLanguage(string prompt)
        {
            var response = await _httpClient.PostAsync("/api/GetFhirQuery", new StringContent(JsonConvert.SerializeObject(new
            {
                prompt = prompt
            }), Encoding.Default, "application/json"));
        }
    }
}

