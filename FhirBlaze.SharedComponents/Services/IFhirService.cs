using System.Collections.Generic;
using System.Threading.Tasks;

namespace FhirBlaze.SharedComponents.Services
{
    public interface IFhirService
    {
        Task<Hl7.Fhir.Model.Patient> CreatePatientsAsync(Hl7.Fhir.Model.Patient Patient);

        Task<IList<Hl7.Fhir.Model.Patient>> GetPatientsAsync();
    }
}