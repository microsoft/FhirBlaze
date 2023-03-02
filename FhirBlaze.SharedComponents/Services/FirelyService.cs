using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bundle = Hl7.Fhir.Model.Bundle;

namespace FhirBlaze.SharedComponents.Services
{
  public class FirelyService : IFhirService
  {
    private readonly int _defaultPageSize = 50;

    private FhirClient _fhirClient;

    public FirelyService(FhirClient client)
    {
      _fhirClient = client;
    }

    public async Task<TResource> GetResourceByIdAsync<TResource>(string resourceId) where TResource : Resource, new()
    {
      var result = await _fhirClient.SearchByIdAsync<TResource>(resourceId, pageSize: _defaultPageSize);

      TResource r = result.Entry.Select(e => (TResource)e.Resource).First();
      return r;
    }

    public async Task<List<TResource>> ExecuteFhirQueryAsync<TResource>(string queryStr) where TResource : Resource, new()
    {
      // assuming query string for multiple resources at this juncture for simple mvp
      var result = await _fhirClient.GetAsync(queryStr);

      Bundle bundle;

      // todo: test below, get new outputs to produce toast messages showing the errors
      try
      {
        bundle = result as Bundle;
      }
      catch (InvalidCastException exception)
      {
        var outcome = result as OperationOutcome;

        throw outcome.ToException();
      }

      List<TResource> resources;

      try
      {
        resources = bundle.Entry.Select(e => (TResource)e.Resource).ToList();
      }
      catch (InvalidCastException exception)
      {
        var outcomes = bundle.Entry.Select(e => (OperationOutcome)e.Resource).ToList();

        throw new AggregateException(outcomes.Select(o => o.ToException()));
      }

      return resources;
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

    public async Task<IList<Questionnaire>> SearchQuestionnaire(string title)
    {
      Bundle bundle = new Bundle();
      if (!string.IsNullOrEmpty(title))
      {
        bundle = await _fhirClient.SearchAsync<Questionnaire>(criteria: new[] { $"title:contains={title}" });
      }
      return bundle.Entry.Select(p => (Questionnaire)p.Resource).ToList();

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
        throw new Exception("Unknown patient ID");
      }

      return await _fhirClient.UpdateAsync(patient);
    }

    public async Task<IList<Observation>> GetPatientObservations(string patientId)
    {
      if (string.IsNullOrEmpty(patientId))
      {
        throw new ArgumentNullException("patientId");
      }

      var bundle = await _fhirClient.SearchAsync<Observation>(criteria: new[] { $"subject=Patient/{patientId}" });
      var result = new List<Observation>();

      while (bundle != null)
      {
        result.AddRange(bundle.Entry.Select(p => (Observation)p.Resource).ToList());
        bundle = await _fhirClient.ContinueAsync(bundle);
      }

      return result;
    }

    public async Task<IList<MedicationStatement>> GetPatientMedicationStatements(string patientId)
    {
      if (string.IsNullOrEmpty(patientId))
      {
        throw new ArgumentNullException("patientId");
      }

      var bundle = await _fhirClient.SearchAsync<MedicationStatement>(criteria: new[] { $"subject=Patient/{patientId}" });
      var result = new List<MedicationStatement>();

      while (bundle != null)
      {
        result.AddRange(bundle.Entry.Select(p => (MedicationStatement)p.Resource).ToList());
        bundle = await _fhirClient.ContinueAsync(bundle);
      }

      return result;
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
      return await _fhirClient.CreateAsync<QuestionnaireResponse>(qResponse);
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

    public async Task<Questionnaire> UpdateQuestionnaireAsync(Questionnaire questionnaire)
    {
      return await _fhirClient.UpdateAsync<Questionnaire>(questionnaire);
    }

    #endregion

    #region Practitioners

    public async Task<IList<Practitioner>> GetPractitionersAsync()
    {
      var bundle = await _fhirClient.SearchAsync<Practitioner>(pageSize: 50);
      var result = new List<Practitioner>();
      while (bundle != null)
      {
        result.AddRange(bundle.Entry.Select(p => (Practitioner)p.Resource).ToList());
        bundle = await _fhirClient.ContinueAsync(bundle);
      }

      return result;
    }

    public async Task<int> GetPractitionerCountAsync()
    {
      var bundle = await _fhirClient.SearchAsync<Practitioner>(summary: SummaryType.Count);
      return bundle.Total ?? 0;
    }

    public async Task<IList<Practitioner>> SearchPractitioner(IDictionary<string, string> searchParameters)
    {
      string identifier = searchParameters["identifier"];

      var searchResults = new List<Practitioner>();

      if (!string.IsNullOrEmpty(identifier))
      {
        Bundle bundle = await _fhirClient.SearchByIdAsync<Practitioner>(identifier);

        if (bundle != null)
          searchResults = bundle.Entry.Select(p => (Practitioner)p.Resource).ToList();
      }
      else
      {
        IList<string> filterStrings = new List<string>();
        foreach (var parameter in searchParameters)
        {
          if (!string.IsNullOrEmpty(parameter.Value))
          {
            filterStrings.Add($"{parameter.Key}:contains={parameter.Value}");
          }
        }
        Bundle bundle = await _fhirClient.SearchAsync<Practitioner>(criteria: filterStrings.ToArray<string>());

        if (bundle != null)
          searchResults = bundle.Entry.Select(p => (Practitioner)p.Resource).ToList();
      }

      return searchResults;
    }

    public async Task<Practitioner> CreatePractitionersAsync(Practitioner practitioner)
    {
      return await _fhirClient.CreateAsync(practitioner);
    }

    public async Task<Practitioner> UpdatePractitionerAsync(string practitionerId, Practitioner practitioner)
    {
      if (practitionerId != practitioner.Id)
      {
        throw new Exception("Unknown practitioner ID");
      }

      return await _fhirClient.UpdateAsync(practitioner);
    }
    #endregion
  }
}