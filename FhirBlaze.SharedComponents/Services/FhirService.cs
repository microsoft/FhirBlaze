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
    public class FhirService:IFhirService
    {
        public HttpClient _client { get; set; }
        public FhirJsonSerializer _serializer { get; set; }
        public FhirJsonParser _parser { get; set; }
        public FhirService(HttpClient client)
        {
            _client = client;
            _parser = new FhirJsonParser();
            _serializer = new FhirJsonSerializer();
            
        }
      
        public async Task<IList<Patient>> GetPatientsAsync()
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

        public async Task<string> DoPost(string path, string content)
        {
            string json = "{}";
            try
            {
                var body = new System.Net.Http.StringContent(
                    content,
                    Encoding.UTF8,
                    "application/json"
                    );

                var r = await _client.PostAsync(path,body);
                json = await r.Content.ReadAsStringAsync();

            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
            return json;
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

        public async Task<Patient> CreatePatientsAsync(Patient Patient)
        {
          string pjson= _serializer.SerializeToString(Patient);
          string json = await DoPost("/Patient", pjson);
          return _parser.Parse<Patient>(json);
        }
    }
}