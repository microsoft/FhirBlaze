using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace FhirBlaze.SharedComponents.Services
{
    public class FHIRBlazeServices:IFHIRBlazeServices
    {
        public HttpClient _client { get; set; }
        public FhirJsonParser _parser { get; set; }
        public FHIRBlazeServices(HttpClient client)
        {
            _client = client;
            _parser = new FhirJsonParser();
            
        }

      
        public async Task<IList<Patient>>  GetPatientsAsync()
        {
            var r=await _client.GetAsync("/Patient");
            var js=await r.Content.ReadAsStringAsync();
            
            var bunndle=_parser.Parse<Bundle>(js);
            var ret = new List<Patient>();
            foreach (var item in bun.Entry)
            {
                ret.Add((Patient)item.Resource);
            }
            return ret;
        }
    }
}