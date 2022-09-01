using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.ObservationModule.Pages
{
  [Authorize]
  public partial class ObservationListPage
  {
    private bool _loading = true;

    [Inject]
    private IFhirService FhirService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    public System.Threading.Tasks.Task<AuthenticationState> AuthTask { get; set; }

    private bool ShowSearch { get; set; } = false;

    public IList<Observation> Observations { get; set; } = new List<Observation>();

    protected override async Task OnInitializedAsync()
    {
      _loading = true;
      Observations = await FhirService.GetObservationsAsync();
      _loading = false;
    }

    private void AddObservationClicked()
    {
      NavigationManager.NavigateTo("/observation");
    }

    private void ObservationSelected(Observation observation)
    {
      NavigationManager.NavigateTo($"/observation/{observation.Id}");
    }

    private async Task SearchObservation(IDictionary<string, string> searchParameters)
    {
      try
      {
        _loading = true;
        this.Observations = await FhirService.SearchObservation(searchParameters);
        _loading = false;
      }
      catch (Exception e)
      {
        Console.WriteLine("Error" + e.Message);
      }
    }

  }
}
