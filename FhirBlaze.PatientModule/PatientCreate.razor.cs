using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;

namespace FhirBlaze.PatientModule
{
    public partial class PatientCreate
    {
        [Parameter]
        public EventCallback<Patient> CreatePatient { get; set; }

        [Parameter]
        public bool Processing { get; set; }

        public Patient Patient { get; set; } = new Patient();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatientID { get; set; } = Guid.NewGuid().ToString();
        public string Birthdate { get; set; } = DateTime.Now.AddYears(-29).AddMonths(-2).AddDays(-3).ToString("yyyy-MM-dd");

      

        protected async void SavePatient()
        {
            var PatientName = new HumanName();
            PatientName.Use = Hl7.Fhir.Model.HumanName.NameUse.Usual;
            PatientName.Given.Append(FirstName);
            PatientName.Family = LastName;
            Patient.Name.Add(PatientName);
            var PatientIdentifier = new Hl7.Fhir.Model.Identifier();
            PatientIdentifier.System = "http://hlsemops.microsoft.com";
            PatientIdentifier.Value = PatientID;
            Patient.Active = true;
            Patient.BirthDate = Birthdate;
            Patient.Identifier = new List<Hl7.Fhir.Model.Identifier>();
            Patient.Identifier.Add(PatientIdentifier);
            await CreatePatient.InvokeAsync(Patient);
        }


    }
}
