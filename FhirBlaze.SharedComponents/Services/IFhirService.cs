using System.Collections.Generic;
using System.Threading.Tasks;

namespace FhirBlaze.SharedComponents.Services
{
    public interface IFhirService
    {
        Task<IList<Hl7.Fhir.Model.Patient>> GetPatientsAsync();
    }
}