using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;

namespace FhirBlaze.PatientModule
{
    public class FHIRService
    {
        private FhirClient _client  { get; set; }

        public FHIRService(string endpoint)
        {
            _client = new FhirClient(endpoint);
        }

    }
}
