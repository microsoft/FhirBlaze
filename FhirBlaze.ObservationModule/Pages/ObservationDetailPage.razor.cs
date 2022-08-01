using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.ObservationModule.Pages
{
  [Authorize]
  public partial class ObservationDetailPage
  {
    [Inject]
    private IFhirService FhirService { get; set; }

    [Parameter]
    public string Id { get; set; }

    public List<Patient> Patients { get; set; }

    public List<Practitioner> Practitioners { get; set; }

    public List<MedicationStatement> Statements { get; set; }

    public ValueSet ValuesetCodes { get; set; }

    private Observation SelectedObservation { get; set; } = new Observation();

    protected override async Task OnParametersSetAsync()
    {
      try
      {
        this.Patients = new List<Patient>(await FhirService.GetPatientsAsync());
        this.Practitioners = new List<Practitioner>(await FhirService.GetPractitionersAsync());
        this.Statements = new List<MedicationStatement>(await FhirService.GetMedicationStatementsAsync());
        this.ValuesetCodes = await FhirService.GetResourceByIdAsync<ValueSet>("observation-codes");

        if (this.Id != null)
        {
          this.SelectedObservation = await FhirService.GetResourceByIdAsync<Observation>(this.Id);
        }
        else
        {
          this.SelectedObservation = new Observation();
        }
      }
      catch (System.Exception e)
      {
        Console.WriteLine("Error in ObservationDetailPage.razor.cs: " + e.Message + "; Source: " + e.Source);
      }
    }

    private async Task SaveObservation(Observation observation)
    {
      Observation persistedObservation = new Observation();
      try
      {
        if (string.IsNullOrEmpty(observation.Id))
        {
          persistedObservation = await FhirService.CreateObservationsAsync(observation);
        }
        else
        {
          persistedObservation = await FhirService.UpdateObservationAsync(observation.Id, observation);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Exception: {ex.Message}");
      }

      this.SelectedObservation = persistedObservation;
    }
  }
}
