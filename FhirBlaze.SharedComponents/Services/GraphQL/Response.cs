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
        [JsonPropertyName("patientList")]
        public IList<JsonDocument> patientList { get; set; }
        

        [JsonPropertyName("whoAmI")]
        public string WhoAmI { get; set; }

      

    }



}
