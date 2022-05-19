using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.MedicationModule.Pages
{
  [Authorize]
  public partial class MedicationStatementDetailPage
  {
    [Inject]
    private IFhirService FhirService { get; set; }

    [Parameter]
    public string Id { get; set; }

    public List<Patient> Patients { get; set; }

    public List<Medication> Medications { get; set; }

    private MedicationStatement SelectedMedicationStatement { get; set; } = new MedicationStatement();

    protected override async Task OnParametersSetAsync()
    {
      try
      {
        this.Patients = new List<Patient>(await FhirService.GetPatientsAsync());
        this.Medications = new List<Medication>(await FhirService.GetMedicationsAsync());

        if (this.Id != null)
        {
          this.SelectedMedicationStatement = await FhirService.GetResourceByIdAsync<MedicationStatement>(this.Id);
        }
        else
        {
          this.SelectedMedicationStatement = new MedicationStatement();
        }
      }
      catch (System.Exception e)
      {
        Console.WriteLine("Error in MedicationStatementDetailPage.razor.cs: " + e.Message + "; Source: " + e.Source);
      }
    }

    private async Task SaveMedicationStatement(MedicationStatement statement)
    {
      MedicationStatement persistedMedicationStatement = new MedicationStatement();
      try
      {
        if (string.IsNullOrEmpty(statement.Id))
        {
          persistedMedicationStatement = await FhirService.CreateMedicationStatementsAsync(statement);
        }
        else
        {
          persistedMedicationStatement = await FhirService.UpdateMedicationStatementAsync(statement.Id, statement);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Exception: {ex.Message}");
      }

      this.SelectedMedicationStatement = persistedMedicationStatement;
    }
  }
}
