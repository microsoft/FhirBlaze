using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace FhirBlaze.ObservationModule.Components
{
  public partial class ObservationListComponent
  {
    [Parameter]
    public EventCallback<Observation> OnObservationSelected { get; set; }

    [CascadingParameter]
    public IList<Observation> Observations { get; set; } = new List<Observation>();

    private void ObservationSelected(Observation observation)
    {
      OnObservationSelected.InvokeAsync(observation);
    }
  }
}
