using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;

namespace FhirBlaze.PatientModule
{
    public partial class PatientForm
    {
        [Parameter]
        public EventCallback<Patient> PersistPatient { get; set; }

        public bool Processing { get; set; }

        [Parameter]
        public Patient? FhirPatient { get; set; }
             
        protected void AddName()
        {
            var NewName = new HumanName();
            NewName.Use = HumanName.NameUse.Usual;
            NewName.GivenElement.Add(new FhirString("First Name"));
            NewName.Family = "Last Name";
            FhirPatient.Name.Add(NewName);
            
        }

        protected void AddID()
        {
            FhirPatient.Identifier.Add(new Identifier("http://hlsemops.microsoft.com", Guid.NewGuid().ToString()));
        }
        protected async void SavePatient()
        {
            Processing = true;
            await PersistPatient.InvokeAsync(FhirPatient);
            Processing = false;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Processing = false;
            if(FhirPatient == null)
            {
                FhirPatient=new Patient();
            }
            if (string.IsNullOrEmpty(FhirPatient.BirthDate))
            {
                FhirPatient.BirthDate = DateTime.Now.AddDays(DateTime.Now.Second).AddMonths(DateTime.Now.Hour).AddYears(-DateTime.Now.Second).ToString("yyyy-MM-dd");
            }  
            if (FhirPatient.Identifier.Count < 1)
            {
                AddID();
            }
            if(FhirPatient.Name.Count < 1)
            {
                AddName();
            }
        }


    }
}
