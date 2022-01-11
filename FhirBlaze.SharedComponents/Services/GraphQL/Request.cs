using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace FhirBlaze.SharedComponents.Services.GraphQL
{
    public class GraphQLRequest
    {
        private HttpClient _httpClient;

        [JsonPropertyName("OperationName")]
        public string OperationName { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("variables")]
        public Dictionary<string, object> Variables { get; set; }

        public GraphQLRequest(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GraphQLResponse> PostAsync()
        {
            var query = new StringContent(
                JsonSerializer.Serialize(this),
                Encoding.UTF8,
                "application/json");
            GraphQLResponse data = null;
            try
            {
                var response = await _httpClient.PostAsync("", query);
                var stream = await response.Content.ReadAsStreamAsync();
                data = await JsonSerializer.DeserializeAsync<GraphQLResponse>(stream);
                if (data.Errors != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in data.Errors)
                    {
                        sb.Append($"{item.message}\n");
                    }

                    throw new ApplicationException(sb.ToString());
                }

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }

            return data;
        }
    }
}
