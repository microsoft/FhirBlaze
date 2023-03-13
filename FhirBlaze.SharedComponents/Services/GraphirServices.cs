using FhirBlaze.SharedComponents.Services.GraphQL;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FhirBlaze.SharedComponents.Services
{
  public class GraphirServices : IFhirService
  {
    private readonly HttpClient _httpClient;
    private readonly FhirJsonParser _fhirParser;
    public GraphirServices(HttpClient httpClient)
    {
      _httpClient = httpClient;
      _fhirParser = new FhirJsonParser();
    }

    public Task<List<TResource>> ExecuteFhirQueryAsync<TResource>(string queryStr) where TResource : Resource, new()
    {
      throw new NotImplementedException();
    }

    public Task<Patient> CreatePatientsAsync(Patient patient)
    {
      throw new NotImplementedException();
    }

    public Task<Practitioner> CreatePractitionersAsync(Practitioner practitioner)
    {
      throw new NotImplementedException();
    }

    public Task<Questionnaire> CreateQuestionnaireAsync(Questionnaire questionnaire)
    {
      throw new NotImplementedException();
    }

    public Task<int> GetPatientCountAsync()
    {
      throw new NotImplementedException();
    }

    public async Task<IList<Patient>> GetPatientsAsync()
    {
      GraphQLRequest request = new GraphQLRequest(_httpClient)
      {
        OperationName = "PatientList",
        Query = @"query {
                    PatientList{
                        identifier{value}    
                        id name{text family given} birthDate
                    }
                }"
      };
      GraphQLResponse response = await request.PostAsync();
      var result = new List<Patient>();
      foreach (var p in response.Data.PatientList)
      {
        try
        {
          result.Add(_fhirParser.Parse<Patient>(p.RootElement.ToString()));
        }
        catch (Exception e)
        {

        }
      }
      return result;

    }

    public Task<int> GetPractitionerCountAsync()
    {
      throw new NotImplementedException();
    }

    public async Task<IList<Practitioner>> GetPractitionersAsync()
    {
      GraphQLRequest request = new GraphQLRequest(_httpClient)
      {
        OperationName = "PractitionerList",
        Query = @"query {
                          PractitionerList {
                            name {
                              given family
                            }
                            id
                            birthDate
                          }
                        }"
      };

      GraphQLResponse response = await request.PostAsync();
      var result = new List<Practitioner>();
      foreach (var p in response.Data.PractitionerList)
      {
        try
        {
          result.Add(_fhirParser.Parse<Practitioner>(p.RootElement.ToString()));
        }
        catch (Exception e)
        {

        }
      }
      return result;
    }

    public Task<Questionnaire> GetQuestionnaireByIdAsync(string id)
    {
      throw new NotImplementedException();
    }

    public Task<QuestionnaireResponse> GetQuestionnaireResponseByIdAsync(string id)
    {
      throw new NotImplementedException();
    }

    public Task<IList<QuestionnaireResponse>> GetQuestionnaireResponsesByQuestionnaireIdAsync(string questionnaireId)
    {
      throw new NotImplementedException();
    }

    public Task<IList<Questionnaire>> GetQuestionnairesAsync()
    {
      throw new NotImplementedException();
    }

    public Task<TResource> GetResourceByIdAsync<TResource>(string resourceId) where TResource : Resource, new()
    {
      throw new NotImplementedException();
    }

    public Task<QuestionnaireResponse> SaveQuestionnaireResponseAsync(QuestionnaireResponse qResponse)
    {
      throw new NotImplementedException();
    }

    public Task<IList<Patient>> SearchPatient(Patient patient)
    {
      throw new NotImplementedException();
    }

    public Task<IList<Practitioner>> SearchPractitioner(IDictionary<string, string> searchParameters)
    {
      throw new NotImplementedException();
    }

    public Task<IList<Questionnaire>> SearchQuestionnaire(string title)
    {
      throw new NotImplementedException();
    }

    public Task<Patient> UpdatePatientAsync(string patientId, Patient patient)
    {
      throw new NotImplementedException();
    }

    public Task<Practitioner> UpdatePractitionerAsync(string practitionerId, Practitioner practitioner)
    {
      throw new NotImplementedException();
    }

    public Task<Questionnaire> UpdateQuestionnaireAsync(Questionnaire questionnaire)
    {
      throw new NotImplementedException();
    }

    public async Task<string> WhoAmIAsync()
    {
      GraphQLRequest request = new GraphQLRequest(_httpClient)
      {
        OperationName = "WhoAMI",
        Query = @"query{whoAmI{}}"
      };

      GraphQLResponse response = await request.PostAsync();

      return response.Data.WhoAmI;
    }

    Task<IList<Observation>> IFhirService.GetPatientObservations(string patientId)
    {
      throw new NotImplementedException();
    }

    Task<IList<MedicationStatement>> IFhirService.GetPatientMedicationStatements(string patientId)
    {
      throw new NotImplementedException();
    }
  }
}
