using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.PatientModule.models
{
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
            PatientName.Use = Hl7.Fhir.Model.HumanName.NameUse.Usual;
            var namelist = PatientName.Given.ToList();
            namelist.Add(FirstName);
            PatientName.Given = namelist.AsEnumerable();
            PatientName.Family = LastName;
            Patient.Name.Add(PatientName);
            var PatientIdentifier = new Hl7.Fhir.Model.Identifier();
            PatientIdentifier.System = "http://hlsemops.microsoft.com";
            PatientIdentifier.Value = PatientID;
            Patient.Active = true;
            Patient.BirthDate = Birthdate.ToString("yyyy-MM-dd");
            Patient.Identifier = new List<Hl7.Fhir.Model.Identifier>();
            Patient.Identifier.Add(PatientIdentifier);
            return Patient;
        }
    }
}
