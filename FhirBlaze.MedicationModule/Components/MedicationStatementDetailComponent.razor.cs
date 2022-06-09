﻿using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace FhirBlaze.MedicationModule.Components
{
  public partial class MedicationStatementDetailComponent
  {
    [Parameter]
    public EventCallback<MedicationStatement> OnMedicationStatementSaved { get; set; }

    [Parameter]
    public bool Processing { get; set; }

    [Parameter]
    public bool IsEditable { get; set; } = true;

    [Parameter]
    public List<Patient> Patients { get; set; }

    [Parameter]
    public List<Medication> Medications { get; set; }

    [CascadingParameter]
    public MedicationStatement Statement { get; set; }

    public string SelectedPatientId
    {
      get
      {
        try
        {
          if (this.Statement.Subject != null)
          {
            foreach (var patient in this.Patients)
            {
              if ($"Patient/{patient.Id}" == this.Statement.Subject.Reference)
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
          this.Statement.Subject = new ResourceReference($"Patient/{value}");
        }
        catch (System.Exception e)
        {
          Console.WriteLine("Error in setSelectedPatientId from MedicationStatementDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }
      }
    }

    public string SelectedMedicationId
    {
      get
      {
        try
        {
          if (this.Statement.Medication != null)
          {
            foreach (var medRes in this.Statement.Contained)
            {
              foreach (var medication in this.Medications)
              {
                if (medication.Id == medRes.Id)
                {
                  return medication.Id;
                }
              }
            }
          }
        }
        catch (System.Exception e)
        {
          Console.WriteLine("Error in getSelectedMedicationId from MedicationStatementDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }

        return "0";
      }
      set
      {
        try
        {
          var medication = this.Medications[this.medicationIndexFromId(value)];

          if (this.Statement.Contained.Count == 0 || this.Statement.Contained[0].Id != value)
          {
            medication.Text = null;
            this.Statement.Contained.Add(medication);
          }

          this.Statement.Medication = new ResourceReference($"#{value}");
        }
        catch (System.Exception e)
        {
          Console.WriteLine("Error in setSelectedMedicationId from MedicationStatementDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }
      }
    }

    public string[] StatementStatuses = Enum.GetNames(typeof(MedicationStatement.MedicationStatusCodes));

    public string SelectedStatus
    {
      get
      {
        return $"{this.Statement.Status}";
      }
      set
      {
        try
        {
          MedicationStatement.MedicationStatusCodes statusCode;

          if (Enum.TryParse<MedicationStatement.MedicationStatusCodes>(value, true, out statusCode))
          {
            Console.WriteLine($"Status value set: {value}");
            this.Statement.Status = statusCode;
          }
          else
          {
            Console.WriteLine("Status value not set: " + value);
          }
        }
        catch (Exception e)
        {
          Console.WriteLine("Error in setSelectedStatus from MedicationStatementDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }
      }
    }

    public Dictionary<string, string> categories = new Dictionary<string, string>() {
      {"inpatient", "Inpatient"},
      {"outpatient", "Outpatient"},
      {"community", "Community"},
      {"patientspecified", "Patient Specified"}
    };

    public string SelectedCategory
    {
      get
      {
        if (this.Statement.Category != null && this.Statement.Category.Coding != null)
        {
          return $"{this.Statement.Category.Coding[0].Code}";
        }
        
        return "";
      }
      set
      {
        try
        {
          this.Statement.Category = this.GeneratedCodeConcept(value, this.categories[value]);
        }
        catch (Exception e)
        {
          Console.WriteLine("Error in setSelectedCategory from MedicationStatementDetailComponent.razor.cs: " + e.Message + "; Source: " + e.Source + "; StackTrace: " + e.StackTrace);
        }
      }
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

    private int medicationIndexFromId(string Id = null)
    {
      for (var index = 0; index < this.Medications.Count; index++)
      {
        if (Id == this.Medications[index].Id || this.SelectedMedicationId == this.Medications[index].Id)
        {
          return index;
        }
      }

      return -1;
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
      Patient patient = this.Patients[this.patientIndexFromId()];
      Medication medication = this.Medications[this.medicationIndexFromId()];

      string rootBlock = "<div xmlns=\"http://www.w3.org/1999/xhtml\">";
      string headingBlock = "<p><b>Generated Narrative with Details</b></p>";
      string idBlock = $"<p><b>id</b>: {this.Statement.Id}</p>";

      string containedBlock = $"<p><b>contained</b>: </p>";

      string identifier = (this.Statement.Identifier.Count > 0) ? $"{this.Statement.Identifier[0]} (OFFICIAL)" : "";
      string identifierBlock = $"<p><b>identifier</b>: {identifier}</p>";

      string statusBlock = $"<p><b>status</b>: {this.Statement.Status}</p>";

      string category = (this.Statement.Category != null && this.Statement.Category.Coding != null) ? $"{this.Statement.Category.Coding[0].Display}" : "";
      string categoryBlock = $"<p><b>category</b>: {category}</p>";

      string medicationBlock = $"<p><b>medication</b>: id: {medication.Id}; ";
      medicationBlock += $"{medication.Code.Coding[0].Display} <span>(Details : {{{medication.Code.Coding[0].System} code {medication.Code.Coding[0].Code}, given as '{medication.Code.Coding[0].Display}'}})</span>; ";
      medicationBlock += $"{medication.Form.Coding[0].Display} <span>(Details : {{SNOMED CT code '{medication.Form.Coding[0].Code}', given as '{medication.Form.Coding[0].Display}'}})</span></p>";

      string subjectBlock = $"<b>subject</b>: {patient.Name[0].ToString()}";

      string endBlock = "</div>";

      return new Narrative()
      {
        Status = Narrative.NarrativeStatus.Generated,
        Div = rootBlock + headingBlock + idBlock + containedBlock + identifierBlock + statusBlock + categoryBlock + medicationBlock + subjectBlock + endBlock
      };
    }

    protected async void SaveMedicationStatement()
    {
      this.Statement.Text = this.GeneratedText();
      await OnMedicationStatementSaved.InvokeAsync(this.Statement);
    }
  }
}
