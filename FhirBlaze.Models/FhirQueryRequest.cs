namespace FhirBlaze.Models;

public class FhirQueryRequest
{
	public string Prompt { get; set; }
	public AiService Service { get; set; }
	public string Model { get; set; }
	public decimal Temperature { get; set; } = 0;
	public decimal TopP { get; set; } = 0;
}

