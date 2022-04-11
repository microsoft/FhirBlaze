using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace FhirBlaze.MedicationModule.Components
{
  public partial class MedicationListComponent
  {
    [Parameter]
    public EventCallback<Medication> OnMedicationSelected { get; set; }

    [CascadingParameter]
    public IList<Medication> Medications { get; set; } = new List<Medication>();

    private void MedicationSelected(Medication medication)
    {
      OnMedicationSelected.InvokeAsync(medication);
    }
  }
}
