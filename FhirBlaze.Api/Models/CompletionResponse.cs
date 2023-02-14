using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FhirBlaze.Api.Models
{
	public class CompletionResponse
	{
		public string Id { get; set; }
		public string Object { get; set; }
		public long Created { get; set; }
		public string Model { get; set; }
		public List<CompletionResponseChoice> Choices { get; set; } = new List<CompletionResponseChoice>();
		public CompletionResponseUsage Usage { get; set; }
	}

	public class CompletionResponseChoice
	{
		public string Text { get; set; }
		public int Index { get; set; }
		[JsonProperty(PropertyName = "logprobs")]
		public string LogProbs { get; set; }
		[JsonProperty(PropertyName = "finish_reason")]
		public string FinishReason { get; set; }
	}

	public class CompletionResponseUsage
	{
        [JsonProperty(PropertyName = "prompt_tokens")]
		public int PromptTokens { get; set; }
        [JsonProperty(PropertyName = "completion_tokens")]
		public int CompletionTokens { get; set; }
        [JsonProperty(PropertyName = "total_tokens")]
		public int TotalTokens { get; set; }
    }
}

