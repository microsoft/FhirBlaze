using Hl7.Fhir.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FhirBlaze.SharedComponents.Services
{
  public interface IFhirService
  {
    Task<TResource> GetResourceByIdAsync<TResource>(string resourceId) where TResource : Hl7.Fhir.Model.Resource, new();

    #region Patient
    Task<Patient> CreatePatientsAsync(Patient patient);
    Task<IList<Patient>> GetPatientsAsync();
    Task<Patient> UpdatePatientAsync(string patientId, Patient patient);
    Task<int> GetPatientCountAsync();
    Task<IList<Patient>> SearchPatient(Patient patient);
    #endregion

    #region Medication
    Task<Medication> CreateMedicationsAsync(Medication medication);
    Task<IList<Medication>> GetMedicationsAsync();
    Task<Medication> UpdateMedicationAsync(string medicationId, Medication medication);
    Task<int> GetMedicationCountAsync();
    Task<IList<Medication>> SearchMedication(IDictionary<string, string> searchParameters);
    #endregion

    #region Questionnaire
    Task<QuestionnaireResponse> SaveQuestionnaireResponseAsync(QuestionnaireResponse qResponse);
    Task<IList<Questionnaire>> GetQuestionnairesAsync();
    Task<Questionnaire> GetQuestionnaireByIdAsync(string id);
    Task<Questionnaire> CreateQuestionnaireAsync(Questionnaire questionnaire);
    Task<QuestionnaireResponse> GetQuestionnaireResponseByIdAsync(string id);
    Task<IList<QuestionnaireResponse>> GetQuestionnaireResponsesByQuestionnaireIdAsync(string questionnaireId);
    Task<Questionnaire> UpdateQuestionnaireAsync(Questionnaire questionnaire);

    Task<IList<Questionnaire>> SearchQuestionnaire(string title);
    #endregion

    #region Practitioners
    Task<IList<Practitioner>> GetPractitionersAsync();

    Task<int> GetPractitionerCountAsync();

    Task<IList<Practitioner>> SearchPractitioner(IDictionary<string, string> searchParameters);

    Task<Practitioner> CreatePractitionersAsync(Practitioner practitioner);

    Task<Practitioner> UpdatePractitionerAsync(string practitionerId, Practitioner practitioner);
    #endregion
  }
}
