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

        [CascadingParameter]
        public Medication Medication { get; set; }

        public string MedicationCode {
          get
          {
            if (this.Medication.Code != null && this.Medication.Code.Coding != null)
            {
              return this.Medication.Code.Coding[0].Code;
            }
            else
            {
              return string.Empty;
            }
          }
          set
          {
            if (this.Medication.Code != null && this.Medication.Code.Coding != null)
            {
              this.Medication.Code.Coding[0].Code = value;
            }
          }
        }
        public string MedicationName
        {
          get
          {
            if (this.Medication.Code != null && this.Medication.Code.Coding != null)
            {
              return this.Medication.Code.Coding[0].Display;
            }
            else
            {
              return string.Empty;
            }
          }
          set
          {
            if (this.Medication.Code != null && this.Medication.Code.Coding != null)
            {
              this.Medication.Code.Coding[0].Display = value;
            }
            else
            {
              Coding CodingItem = new Coding("http://snomed.info/sct", this.MedicationCode, value);

              List<Coding> CodingList = new List<Coding>();
              CodingList.Add(CodingItem);

              CodeableConcept Code = new CodeableConcept();
              Code.Coding = CodingList;

              this.Medication.Code = Code;
            }
          }
        }

        public string MedicationForm
        {
          get
          {
            if (this.Medication.Form != null && this.Medication.Form.Coding != null)
            {
              return this.Medication.Form.Coding[0].Display;
            }
            else
            {
              return string.Empty;
            }
          }
          set
          {
            if (this.Medication.Form != null && this.Medication.Form.Coding != null)
            {
              this.Medication.Form.Coding[0].Display = value;
            }
            else
            {
              Coding CodingItem = new Coding("http://snomed.info/sct", this.MedicationCode, value);

              List<Coding> CodingList = new List<Coding>();
              CodingList.Add(CodingItem);

              CodeableConcept Code = new CodeableConcept();
              Code.Coding = CodingList;

              this.Medication.Form = Code;
            }
          }
        }

        private Narrative GeneratedText()
        {
          string rootBlock = "<div xmlns=\"http://www.w3.org/1999/xhtml\">";
          string headingBlock = "<p><b>Generated Narrative with Details</b></p>";
          string idBlock = $"<p><b>id</b>: {this.Medication.Id}</p>";
          string containedBlock = $"<p><b>contained</b>: </p><p><b>code</b>: {this.MedicationName} <span>(Details : {{{this.Medication.Code.Coding[0].System} code {this.Medication.Code.Coding[0].Code}, given as '{this.Medication.Code.Coding[0].Display}'}})</span></p>";
          string endBlock = "</div>";

          return new Narrative()
          {
            Status = Enum.Parse<Narrative.NarrativeStatus>("generated", true),
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
