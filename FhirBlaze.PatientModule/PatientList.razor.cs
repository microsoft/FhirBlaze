using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.PatientModule
{
    [Authorize]
    public partial class PatientList
    {
        [Inject]
        public IFhirService FBService { get; set; }
        
        protected bool ShowCreate { get; set; } = false;
        protected bool Loading { get; set; } = true;
        public IList<Patient> Patients { get; set; } = new List<Patient>();

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            
            Loading = true;
            await base.OnInitializedAsync();
            Patients = await FBService.GetPatientsAsync();
            Loading = false;
            ShouldRender();
        }

        public void ToggleCreate()
        {
            ShowCreate = !ShowCreate;
        }
    }
}
