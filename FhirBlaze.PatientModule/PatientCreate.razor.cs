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

        [Parameter]
        public models.SimplePatient Patient { get; set; }
             

        protected async void SavePatient()
        {  
            await CreatePatient.InvokeAsync(Patient.ToHl7Patient());
        }


    }
}
