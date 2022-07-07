using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.MedicationModule.Pages
{
  [Authorize]
  public partial class MedicationStatementListPage
  {
    private bool _loading = true;

    [Inject]
    private IFhirService FhirService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    public System.Threading.Tasks.Task<AuthenticationState> AuthTask { get; set; }

    private bool ShowSearch { get; set; } = false;

    public IList<MedicationStatement> MedicationStatements { get; set; } = new List<MedicationStatement>();

    protected override async Task OnInitializedAsync()
    {
      _loading = true;
      MedicationStatements = await FhirService.GetMedicationStatementsAsync();
      _loading = false;
    }

    private void AddMedicationStatementClicked()
    {
      NavigationManager.NavigateTo("/statement");
    }

    private void MedicationStatementSelected(MedicationStatement statement)
    {
      NavigationManager.NavigateTo($"/statement/{statement.Id}");
    }

    private async Task SearchMedicationStatement(IDictionary<string, string> searchParameters)
    {
      try
      {
        _loading = true;
        this.MedicationStatements = await FhirService.SearchMedicationStatement(searchParameters);
        _loading = false;
      }
      catch (Exception e)
      {
        Console.WriteLine("Error" + e.Message);
      }
    }

  }
}
