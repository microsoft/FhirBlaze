using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FhirBlaze.SharedComponents.Services
{
    public class FirelyService:IFhirService
    {
        private FhirClient _fhirClient;
        public FirelyService(FhirClient client)
        {
            _fhirClient = client;
        }



        #region Patient
        public async Task<IList<Patient>> GetPatientsAsync()
        {
            var bundle = await _fhirClient.SearchAsync<Patient>(pageSize: 50);
            var result = new List<Patient>();
            while (bundle != null)
            {
                result.AddRange(bundle.Entry.Select(p => (Patient)p.Resource).ToList());
                bundle = await _fhirClient.ContinueAsync(bundle);
            }

            return result;
        }
       
        public async Task<int> GetPatientCountAsync()
        {
            var bundle = await _fhirClient.SearchAsync<Patient>(summary: SummaryType.Count);
            return bundle.Total ?? 0;
        }

        public async Task<IList<Patient>> SearchPatient(Patient Patient)
        {
            string givenName = ""; //The given name is not working on the mapping
            string familyName = Patient.Name[0].Family;
            string identifier = Patient.Identifier[0].Value;
            Bundle bundle;

            if (!string.IsNullOrEmpty(identifier))
            {
                bundle = await _fhirClient.SearchByIdAsync<Patient>(identifier);

                if (bundle != null)
                    return bundle.Entry.Select(p => (Patient)p.Resource).ToList();
            }

            if (!string.IsNullOrEmpty(familyName))
            {
                bundle = await _fhirClient.SearchAsync<Patient>(criteria: new[] { $"family:contains={familyName}" });

                if (bundle != null)
                    return bundle.Entry.Select(p => (Patient)p.Resource).ToList();
            }

            return await GetPatientsAsync();           
        }

        public async Task<Patient> CreatePatientsAsync(Patient patient)
        {
            return await _fhirClient.CreateAsync(patient);
        }

        public async Task<Patient> UpdatePatientAsync(string patientId, Patient patient)
        {
            if (patientId != patient.Id)
            {
                throw new System.Exception("Unknown patient ID");
            }

            return await _fhirClient.UpdateAsync(patient);
        }
        #endregion

        #region Questionnaire
        public async Task<IList<Questionnaire>> GetQuestionnairesAsync()
        {
            var bundle = await _fhirClient.SearchAsync<Questionnaire>(pageSize: 100);
            var results = new List<Questionnaire>();

            while (bundle != null)
            {
                results.AddRange(bundle.Entry.Select(q => (Questionnaire)q.Resource));
                bundle = await _fhirClient.ContinueAsync(bundle);
            }

            return results;
        }

        public async Task<Questionnaire> GetQuestionnaireByIdAsync(string id)
        {
            var result = await _fhirClient.ReadAsync<Questionnaire>($"Questionnaire/{id}");
            if (result == null)
                throw new System.Exception($"{id} was not found.");

            return result;
        }

        public async Task<Questionnaire> CreateQuestionnaireAsync(Questionnaire questionnaire)
        {
            return await _fhirClient.CreateAsync<Questionnaire>(questionnaire);
        }

        public async Task<QuestionnaireResponse> SaveQuestionnaireResponseAsync(QuestionnaireResponse qResponse)
        {
            throw new System.NotImplementedException();
        }

        public async Task<QuestionnaireResponse> GetQuestionnaireResponseByIdAsync(string id)
        {
            var result = await _fhirClient.ReadAsync<QuestionnaireResponse>($"QuestionnaireResponse/{id}");
            if (result == null)
                throw new System.Exception($"{id} was not found.");

            return result;
        }

        public async Task<IList<QuestionnaireResponse>> GetQuestionnaireResponsesByQuestionnaireIdAsync(string questionnaireId)
        {
            var bundle = await _fhirClient.SearchAsync<QuestionnaireResponse>(pageSize: 100);
            var result = new List<QuestionnaireResponse>();
            while (bundle != null)
            {
                result.AddRange(bundle.Entry.Select(qr => (QuestionnaireResponse)qr.Resource).ToList());
                bundle = await _fhirClient.ContinueAsync(bundle);
            }

            return result.Where(r => r.Questionnaire.Contains(questionnaireId)).ToList();
        }

        #endregion

    }
}