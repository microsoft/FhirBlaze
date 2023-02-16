using System;
namespace FhirBlaze.Models
{
	public class FhirQueryRequest
	{
		public string Prompt { get; set; }
		public AiService Service { get; set; }
		public string Model { get; set; }
	}
}

