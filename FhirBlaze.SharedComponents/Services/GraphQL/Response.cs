using Hl7.Fhir.Model;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FhirBlaze.SharedComponents.Services.GraphQL
{


  public class GraphQLResponse
  {
    [JsonPropertyName("data")]
    public Data Data { get; set; }

    [JsonPropertyName("errors")]
    public IList<GQLError> Errors { get; set; }

  }

  public class GQLError
  {
    [JsonPropertyName("message")]
    public string message { get; set; }
  }

  public class Data
  {
    [JsonPropertyName("PatientList")]
    public IList<JsonDocument> PatientList { get; set; }

    [JsonPropertyName("MedicationList")]
    public IList<JsonDocument> MedicationList { get; set; }

    [JsonPropertyName("MedicationStatementList")]
    public IList<JsonDocument> MedicationStatementList { get; set; }

    [JsonPropertyName("ObservationList")]
    public IList<JsonDocument> ObservationList { get; set; }

    [JsonPropertyName("whoAmI")]
    public string WhoAmI { get; set; }

    [JsonPropertyName("PractitionerList")]
    public IList<JsonDocument> PractitionerList { get; set; }

  }



}
