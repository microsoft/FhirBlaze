using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
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
            string json = await DoGetAsync("/Patient");           
            var bundle=_parser.Parse<Bundle>(json);
            var ret = new List<Patient>();
            foreach (var item in bundle.Entry)
            {
                ret.Add((Patient)item.Resource);
            }
            return ret;
        }

        public async Task<string> DoGetAsync(string path)
        {
            string json = "{}";
            try
            {
                var r = await _client.GetAsync(path);
                json= await r.Content.ReadAsStringAsync();

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
            return json;
        }
    }
}