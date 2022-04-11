using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace FhirBlaze.MedicationModule.Components
{
  public partial class MedicationDetailComponent
  {
    [Parameter]
    public EventCallback<Medication> OnMedicationSaved { get; set; }

    [Parameter]
    public bool Processing { get; set; }

    [Parameter]
    public bool IsEditable { get; set; } = true;

    [Parameter]
    public ValueSet ValueSetMedicationCodes { get; set; }

    [Parameter]
    public ValueSet ValueSetMedicationFormCodes { get; set; }

    [CascadingParameter]
    public Medication Medication { get; set; }

    public string MedicationCode
    {
      get
      {
        if (this.Medication.Code != null && this.Medication.Code.Coding != null)
        {
          var Concept = this.ValueSetMedicationCodes.Compose.Include[0].Concept;

          for (var i = 0; i < Concept.Count; i++)
          {
            if (Concept[i].Code == this.Medication.Code.Coding[0].Code)
            {
              return $"{i}";
            }
          }
        }

        return "0";
      }
      set
      {
        var Concept = this.ValueSetMedicationCodes.Compose.Include[0].Concept[int.Parse(value)];

        if (this.Medication.Code != null && this.Medication.Code.Coding != null)
        {
          this.Medication.Code.Coding[0].Code = Concept.Code;
          this.Medication.Code.Coding[0].Display = Concept.Display;
        }
        else
        {
          this.Medication.Code = GeneratedCodeConcept(Concept.Code, Concept.Display);
        }
      }
    }

    public string MedicationFormCode
    {
      get
      {
        if (this.Medication.Form != null && this.Medication.Form.Coding != null)
        {
          var Concept = this.ValueSetMedicationFormCodes.Compose.Include[0].Concept;

          for (var i = 0; i < Concept.Count; i++)
          {
            if (Concept[i].Code == this.Medication.Form.Coding[0].Code)
            {
              return $"{i}";
            }
          }
        }

        return "0";
      }
      set
      {
        var Concept = this.ValueSetMedicationFormCodes.Compose.Include[0].Concept[int.Parse(value)];

        if (this.Medication.Form != null && this.Medication.Form.Coding != null)
        {
          this.Medication.Form.Coding[0].Code = Concept.Code;
          this.Medication.Form.Coding[0].Display = Concept.Display;
        }
        else
        {
          this.Medication.Form = GeneratedCodeConcept(Concept.Code, Concept.Display);
        }
      }
    }

    private CodeableConcept GeneratedCodeConcept(string Code, string Display)
    {
      Coding CodingItem = new Coding("http://snomed.info/sct", Code, Display);

      List<Coding> CodingList = new List<Coding>();
      CodingList.Add(CodingItem);

      CodeableConcept CodeConcept = new CodeableConcept();
      CodeConcept.Coding = CodingList;

      return CodeConcept;
    }

    private Narrative GeneratedText()
    {
      string rootBlock = "<div xmlns=\"http://www.w3.org/1999/xhtml\">";
      string headingBlock = "<p><b>Generated Narrative with Details</b></p>";
      string idBlock = $"<p><b>id</b>: {this.Medication.Id}</p>";

      string containedBlock = $"<p><b>contained</b>: </p><p><b>code</b>: {this.Medication.Code.Coding[0].Display} <span>(Details : {{{this.Medication.Code.Coding[0].System} code {this.Medication.Code.Coding[0].Code}, given as '{this.Medication.Code.Coding[0].Display}'}})</span></p>";
      containedBlock += $"<p><b>form</b>: {this.Medication.Form.Coding[0].Display} <span>(Details : {{SNOMED CT code '{this.Medication.Form.Coding[0].Code}', given as '{this.Medication.Form.Coding[0].Display}'}})</span></p>";
      string endBlock = "</div>";

      return new Narrative()
      {
        Status = Narrative.NarrativeStatus.Generated,
        Div = rootBlock + headingBlock + idBlock + containedBlock + endBlock
      };
    }

    protected async void SaveMedication()
    {
      this.Medication.Text = this.GeneratedText();
      await OnMedicationSaved.InvokeAsync(this.Medication);
    }
  }
}
