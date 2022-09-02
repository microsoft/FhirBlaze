using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using BlazorDateRangePicker;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.ObservationModule.Components
{
  public partial class ObservationDetailComponent
  {
    [Parameter]
    public EventCallback<Observation> OnObservationSaved { get; set; }

    [Parameter]
    public bool Processing { get; set; }

    [Parameter]
    public bool IsEditable { get; set; } = true;

    [Parameter]
    public List<Patient> Patients { get; set; } = new List<Patient>();

    [Parameter]
    public List<Practitioner> Practitioners { get; set; } = new List<Practitioner>();

    [Parameter]
    public List<MedicationStatement> Statements { get; set; } = new List<MedicationStatement>();

    [Parameter]
    public ValueSet ObservationCodes { get; set; } = new ValueSet();

    [CascadingParameter]
    public Observation Observation { get; set; } = new Observation();

    public string[] ObservationStatuses = Enum.GetNames(typeof(ObservationStatus));

    public string ObservationStatus
    {
      get
      {
        return $"{this.Observation.Status}";
      }
      set
      {
        try
        {
          ObservationStatus statusCode;

          if (Enum.TryParse<ObservationStatus>(value, true, out statusCode))
          {
            Console.WriteLine($"Status value set: {value}");
            this.Observation.Status = statusCode;
          }
          else
          {
            Console.WriteLine("Status value not set: " + value);
          }
        }
        catch (Exception e)
        {
          Console.WriteLine("Error in setSelectedStatus from ObservationDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }
      }
    }

    public string ObservationCode
    {
      get
      {
        if (this.Observation.Code != null && this.Observation.Code.Coding != null)
        {
          var Concept = this.ObservationCodes.Compose.Include[0].Concept;

          for (var i = 0; i < Concept.Count; i++)
          {
            if (Concept[i].Code == this.Observation.Code.Coding[0].Code)
            {
              return $"{i}";
            }
          }
        }

        return "0";
      }
      set
      {
        var Concept = this.ObservationCodes.Compose.Include[0].Concept[int.Parse(value)];

        if (this.Observation.Code != null && this.Observation.Code.Coding != null)
        {
          this.Observation.Code.Coding[0].Code = Concept.Code;
          this.Observation.Code.Coding[0].Display = Concept.Display;
        }
        else
        {
          this.Observation.Code = GeneratedCodeConcept(Concept.Code, Concept.Display);
        }
      }
    }

    public string SelectedPatientId
    {
      get
      {
        try
        {
          if (this.Observation.Subject != null)
          {
            foreach (var patient in this.Patients)
            {
              if ($"Patient/{patient.Id}" == this.Observation.Subject.Reference)
              {
                return patient.Id;
              }
            }
          }
        }
        catch (System.Exception e)
        {
          Console.WriteLine("Error in getSelectedPatientId from MedicationStatementDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }

        return "0";
      }
      set
      {
        try
        {
          var display = this.Patients[this.patientIndexFromId(value)].Name[0].ToString();
          this.Observation.Subject = new ResourceReference($"Patient/{value}", display);
        }
        catch (System.Exception e)
        {
          Console.WriteLine("Error in setSelectedPatientId from MedicationStatementDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }
      }
    }

    public string SelectedPractitionerId
    {
      get
      {
        try
        {
          if (this.Observation.Performer != null)
          {
            foreach (var practitioner in this.Practitioners)
            {
              if (this.Observation.Performer.Count > 0 && $"Practitioner/{practitioner.Id}" == this.Observation.Performer[0].Reference)
              {
                return practitioner.Id;
              }
            }
          }
        }
        catch (System.Exception e)
        {
          Console.WriteLine("Error in getSelectedPractitionerId from ObservationDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }

        return "0";
      }
      set
      {
        try
        {
          var performer = this.Practitioners[this.practitionerIndexFromId(value)];

          if (this.Observation.Performer.Count > 0)
          {
            this.Observation.Performer.RemoveAt(0);
          }

          var refPer = new ResourceReference($"Practitioner/{value}", performer.Name[0].ToString());
          this.Observation.Performer.Add(refPer);
        }
        catch (System.Exception e)
        {
          Console.WriteLine("Error in setSelectedPractitionerId from ObservationDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }
      }
    }

    public string SelectedStatementId
    {
      get
      {
        try
        {
          if (this.Observation.PartOf != null)
          {
            foreach (var refPer in this.Observation.PartOf)
            {
              foreach (var statement in this.Statements)
              {
                if ($"MedicationStatement/{statement.Id}" == refPer.Reference)
                {
                  return statement.Id;
                }
              }
            }
          }
        }
        catch (System.Exception e)
        {
          Console.WriteLine("Error in getSelectedStatementId from ObservationDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }

        return "0";
      }
      set
      {
        try
        {
          var statement = this.Statements[this.statementIndexFromId(value)];

          if (this.Observation.PartOf.Count > 0)
          {
            this.Observation.PartOf.RemoveAt(0);
          }

          var refPer = new ResourceReference($"MedicationStatement/{value}", statement.Category.Coding[0].Code);
          this.Observation.PartOf.Add(refPer);
        }
        catch (System.Exception e)
        {
          Console.WriteLine("Error in setSelectedStatementId from ObservationDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }
      }
    }

    public void SelectDate(DateTimeOffset date)
    {
      this.Observation.Effective = new FhirDateTime(date.ToString("yyyy-MM-dd"));
    }

    public DateTimeOffset? SelectedEffectiveDate
    {
      get
      {
        if (this.Observation.Effective != null)
        {
          return DateTimeOffset.Parse(this.Observation.Effective.ToString());
        }

        return DateTimeOffset.Now;
      }
      set
      {

      }
    }

    public string Annote
    {
      get
      {
        if (this.Observation.Note != null && this.Observation.Note.Count > 0)
        {
          return this.Observation.Note[0].Text.ToString();
        }

        return string.Empty;
      }
      set
      {
        if (this.Observation.Note != null && this.Observation.Note.Count > 0)
        {
          this.Observation.Note[0].Text = new Markdown(value);
        }
        else
        {
          Annotation note = new Annotation();
          note.Text = new Markdown(value);
          this.Observation.Note.Add(note);
        }
      }
    }

    public void AddNoteAnnotation()
    {
      if (this.Observation.Note == null)
      {
        this.Observation.Note = new List<Annotation>();
      }

      this.Observation.Note.Add(new Annotation() { Text = new Markdown() });
    }

    public void RemoveNoteAnnotation(Annotation note)
    {
      this.Observation.Note.Remove(note);
    }

    private int patientIndexFromId(string Id = null)
    {
      for (var index = 0; index < this.Patients.Count; index++)
      {
        if (Id == this.Patients[index].Id || this.SelectedPatientId == this.Patients[index].Id)
        {
          return index;
        }
      }

      return -1;
    }

    private int practitionerIndexFromId(string Id = null)
    {
      for (var index = 0; index < this.Practitioners.Count; index++)
      {
        if (Id == this.Practitioners[index].Id || this.SelectedPractitionerId == this.Practitioners[index].Id)
        {
          return index;
        }
      }

      return -1;
    }

    private int statementIndexFromId(string Id = null)
    {
      for (var index = 0; index < this.Statements.Count; index++)
      {
        if (Id == this.Statements[index].Id || this.SelectedStatementId == this.Statements[index].Id)
        {
          return index;
        }
      }

      return -1;
    }

    private CodeableConcept GeneratedCodeConcept(string Code, string Display)
    {
      Coding CodingItem = new Coding("http://loinc.org", Code, Display);

      List<Coding> CodingList = new List<Coding>();
      CodingList.Add(CodingItem);

      CodeableConcept CodeConcept = new CodeableConcept();
      CodeConcept.Coding = CodingList;

      return CodeConcept;
    }

    private Narrative GeneratedText()
    {
      string rootBlock = "<div xmlns=\"http://www.w3.org/1999/xhtml\">";
      string headingBlock = "<p class=\"obsservation_narrative\"><b>Generated Narrative with Details</b></p>";
      string idBlock = $"<p class=\"obsservation_id\"><b>id</b>: {this.Observation.Id}</p>";

      string identifierBlock = $"<p class=\"obsservation_identifier\"><b>identifier</b>: {(this.Observation.Identifier.Count > 0 ? this.Observation.Identifier[0].Value + "(OFFICIAL)" : "")}</p>";

      string partOfBlock = $"<p class=\"obsservation_part_of\"><b>part of</b>: <a href=\"{this.Observation.PartOf[0].Reference}\">{this.Observation.PartOf[0].Display}</a></p>";

      string statusBlock = $"<p class=\"obsservation_status\"><b>status</b>: {this.Observation.Status}</p>";

      string codeBlock = $"<p class=\"obsservation_code\"><b>code</b>: {this.Observation.Code.Coding[0].Display} <span>(Details : {{{this.Observation.Code.Coding[0].System} code {this.Observation.Code.Coding[0].Code}, given as '{this.Observation.Code.Coding[0].Display}'}})</span></p>";

      // string categoryBlock = $"<p class=\"obsservation_category\"><b>category</b>: {(this.Observation.Category != null && this.Observation.Category.Coding != null ? this.Observation.Category.Coding[0].Display : "")}</p>";

      string subjectBlock = $"<p class=\"obsservation_subject\"><b>subject</b>: <a href=\"{this.Observation.Subject.Reference}\">{this.Observation.Subject.Display}</a></p>";

      string effectiveDateBlock = $"<p class=\"obsservation_effective\"><b>effective</b>: {(this.Observation.Effective != null ? this.Observation.Effective.ToString() : "")}</p>";

      string performerBlock = $"<p class=\"obsservation_performer\"><b>performer</b>: <a href=\"{this.Observation.Performer[0].Reference}\">{this.Observation.Performer[0].Display}</a></p>";

      string noteBlock = string.Empty;

      if (this.Observation.Note != null && this.Observation.Note.Count > 0)
      {
        noteBlock = $"<p class=\"obsservation_note\"><b>note</b>: ";
        foreach (var note in this.Observation.Note)
        {
          noteBlock += $"{note.Text.ToString()} ";
        }
        noteBlock += "</p>";
      }

      string endBlock = "</div>";

      return new Narrative()
      {
        Status = Narrative.NarrativeStatus.Generated,
        Div = rootBlock + headingBlock + idBlock + identifierBlock + partOfBlock + statusBlock + codeBlock + subjectBlock + effectiveDateBlock + performerBlock + noteBlock + endBlock
      };
    }

    protected async void SaveObservation()
    {
      this.Observation.Text = this.GeneratedText();

      if (this.Observation.Issued == null)
      {
        this.Observation.Issued = DateTimeOffset.Now;
      }

      await OnObservationSaved.InvokeAsync(this.Observation);
    }
  }
}
