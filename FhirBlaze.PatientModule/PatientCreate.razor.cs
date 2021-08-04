using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace FhirBlaze.PatientModule
{
    public partial class PatientCreate
    {
        public Patient Patient { get; set; } = new Patient();

        public HumanName PatientName { get; set; } = new HumanName();
        public string FirstName { get; set; }
        protected void CreatePatient()
        {
            PatientName.Given.Append(FirstName);
            Patient.Name.Add(PatientName);
            

        }


    }
}
