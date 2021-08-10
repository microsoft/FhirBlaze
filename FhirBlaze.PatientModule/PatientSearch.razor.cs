using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;

namespace FhirBlaze.PatientModule
{
    public partial class PatientSearch
    {
        [Parameter]
        public EventCallback<Patient> SearchPatient { get; set; } 

        [Parameter]
        public bool Processing { get; set; }

        [Parameter]
        public models.SimplePatient Patient { get; set; }

        protected async void searchPatient()
        {
            try
            {
               await SearchPatient.InvokeAsync(Patient.ToHl7Patient());
            }
            catch (Exception)
            {
                // do nothing
            }
        }

    }
}
