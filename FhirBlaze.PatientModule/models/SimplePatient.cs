using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;

namespace FhirBlaze.PatientModule.Models;

public class SimplePatient
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PatientID { get; set; } 
    public DateTime Birthdate { get; set; } 

    public Patient ToHl7Patient()
    {
        var Patient = new Patient();
        var PatientName = new HumanName();
        PatientName.Use = HumanName.NameUse.Usual;
        var namelist = new[] { FirstName };
        PatientName.Given = namelist;
        PatientName.Family = LastName;
        Patient.Name.Add(PatientName);
        var PatientIdentifier = new Identifier();
        PatientIdentifier.System = "http://hlsemops.microsoft.com";
        PatientIdentifier.Value = PatientID;
        Patient.Active = true;
        Patient.BirthDate = Birthdate.ToString("yyyy-MM-dd");
        Patient.Identifier = new List<Hl7.Fhir.Model.Identifier>();
        Patient.Identifier.Add(PatientIdentifier);
        Patient.Id = PatientID;
        return Patient;
    }

    public Patient UpdateHL7FHIRPatient(Patient updatePatient)
    {
        var PatientName = new HumanName();
        PatientName.Use = HumanName.NameUse.Usual;
        var namelist = new[] { FirstName };
        PatientName.Given = namelist;
        PatientName.Family = LastName;
        updatePatient.Name.RemoveAt(0);
        updatePatient.Name.Add(PatientName);
        updatePatient.BirthDate = Birthdate.ToString("yyyy-MM-dd");
        var PatientIdentifier = new Identifier();
        PatientIdentifier.System = "http://hlsemops.microsoft.com";
        PatientIdentifier.Value = PatientID;
        updatePatient.Id = PatientID;
        return updatePatient;
    }
}
