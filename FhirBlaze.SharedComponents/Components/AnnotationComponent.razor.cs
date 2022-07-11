using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;

namespace FhirBlaze.SharedComponents.Components
{
  public partial class AnnotationComponent
  {
    [Parameter]
    public Annotation Note { get; set; } = new Annotation();

    public string Text
    {
      get => this.Note.Text.ToString();
      set
      {
        this.Note.Text = new Markdown(value);
      }
    }
  }
}
