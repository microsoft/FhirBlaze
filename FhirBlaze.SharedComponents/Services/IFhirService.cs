using System.Collections.Generic;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace FhirBlaze.SharedComponents.Services
{
    public interface IFhirService
    {
        Task<Patient> CreatePatientsAsync(Patient patient);
        Task<IList<Patient>> GetPatientsAsync();
        Task<int> GetPatientCountAsync();
        Task<IList<Patient>> SearchPatient(Patient patient);
        Task<IList<Questionnaire>> GetQuestionnaireAsync();
        Task<Questionnaire> CreateQuestionnaireAsync(Questionnaire questionnaire);
    }
}