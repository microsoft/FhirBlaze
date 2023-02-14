using System;
using Newtonsoft.Json;

namespace FhirBlaze.Api.Models
{
	public class CompletionRequest
	{
        [JsonProperty(PropertyName = "prompt")]
        public string Prompt { get; set; }
        [JsonProperty(PropertyName = "temperature")]
        public decimal Temperature { get; set; }
		[JsonProperty(PropertyName = "top_p")]
		public decimal TopP { get; set; }
		[JsonProperty(PropertyName = "frequency_penalty")]
		public decimal FrequencyPenalty { get; set; }
		[JsonProperty(PropertyName = "presence_penalty")]
		public decimal PresencePenalty { get; set; }
        [JsonProperty(PropertyName = "best_of")]
		public int BestOf { get; set; }
        [JsonProperty(PropertyName = "max_tokens")]
		public int MaxTokens { get; set; }
        [JsonProperty(PropertyName = "stop")]
        public string[]? Stop { get; set; }
    }
}

