using System.Collections.Generic;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace FhirBlaze.SharedComponents.Services
{
    public interface IFhirService
    {
        #region Patient
        Task<Patient> CreatePatientsAsync(Patient patient);
        Task<IList<Patient>> GetPatientsAsync();
        Task<Patient> UpdatePatientAsync(string patientId, Patient patient);
        Task<int> GetPatientCountAsync();
        Task<IList<Patient>> SearchPatient(Patient patient);
        #endregion

        #region Questionnaire
        Task<QuestionnaireResponse> SaveQuestionnaireResponseAsync(QuestionnaireResponse qResponse);
        Task<IList<Questionnaire>> GetQuestionnairesAsync();
        Task<Questionnaire> GetQuestionnaireByIdAsync(string id);
        Task<Questionnaire> CreateQuestionnaireAsync(Questionnaire questionnaire);
        Task<QuestionnaireResponse> GetQuestionnaireResponseByIdAsync(string id);
        Task<IList<QuestionnaireResponse>> GetQuestionnaireResponsesByQuestionnaireIdAsync(string questionnaireId);

        #endregion
    }
}