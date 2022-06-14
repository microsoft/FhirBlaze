using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;

namespace FhirBlaze.MedicationModule.Components
{
  public partial class AnnotationComponent
  {
    [Parameter]
    public Annotation Note { get; set; } = new Annotation();

    private string Text
    {
      get => this.Note.Text.ToString();
      set
      {
        this.Note.Text = new Markdown(value);
      }
    }
  }
}
