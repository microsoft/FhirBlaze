using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace FhirBlaze.MedicationModule.Components
{
  public partial class MedicationStatementListComponent
  {
    [Parameter]
    public EventCallback<MedicationStatement> OnMedicationStatementSelected { get; set; }

    [CascadingParameter]
    public IList<MedicationStatement> MedicationStatements { get; set; } = new List<MedicationStatement>();

    private void MedicationStatementSelected(MedicationStatement statement)
    {
      OnMedicationStatementSelected.InvokeAsync(statement);
    }
  }
}
