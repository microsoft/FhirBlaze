using System.Collections.Generic;
using System.Threading.Tasks;

namespace FhirBlaze.SharedComponents.Services
{
    public interface IFhirService
    {
        Task<Hl7.Fhir.Model.Patient> CreatePatientsAsync(Hl7.Fhir.Model.Patient Patient);

        //Task<Hl7.Fhir.Model.Questionnaire> CreateQuestionnaireAsync(Hl7.Fhir.Model.Questionnaire Questionnaire);
        Task<IList<Hl7.Fhir.Model.Patient>> GetPatientsAsync();

        Task<IList<Hl7.Fhir.Model.Questionnaire>> GetQuestionnaireAsync();
    }
}