using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.MedicationModule.Pages
{
  [Authorize]
  public partial class MedicationDetailPage
  {
    [Inject]
    private IFhirService FhirService { get; set; }

    public ValueSet MedicationCodes { get; set; }

    public ValueSet MedicationFormCodes { get; set; }

    [Parameter]
    public string Id { get; set; }

    private Medication SelectedMedication { get; set; } = new Medication();

    protected override async Task OnParametersSetAsync()
    {
      try
      {
        this.MedicationCodes = await FhirService.GetResourceByIdAsync<ValueSet>("medication-codes");
        this.MedicationFormCodes = await FhirService.GetResourceByIdAsync<ValueSet>("medication-form-codes");

        if (this.Id != null)
        {
          this.SelectedMedication = await FhirService.GetResourceByIdAsync<Medication>(this.Id);
        }
        else
        {
          this.SelectedMedication = new Medication();
        }
      }
      catch (System.Exception e)
      {
        Console.WriteLine("Error in MedicationDetailPage.razor.cs: " + e.Message + "; Source: " + e.Source);
      }
    }

    private async Task SaveMedication(Medication medication)
    {
      Medication persistedMedication = new Medication();
      try
      {
        if (string.IsNullOrEmpty(medication.Id))
        {
          persistedMedication = await FhirService.CreateMedicationsAsync(medication);
        }
        else
        {
          persistedMedication = await FhirService.UpdateMedicationAsync(medication.Id, medication);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Exception: {ex.Message}");
      }

      this.SelectedMedication = persistedMedication;
    }
  }
}
