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
        IFhirService FhirService { get; set; }
        protected bool ShowCreate { get; set; } = false;
        protected bool Loading { get; set; } = true;
        protected bool ProcessingCreate { get; set; } = false;
        
        public IList<Patient> Patients { get; set; } = new List<Patient>();

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {            
            Loading = true;
            await base.OnInitializedAsync();
            Patients = await FhirService.GetPatientsAsync();
            Loading = false;
            ShouldRender();
        }

        public async void CreatePatient(Patient patient)
        {
            try
            {
                ProcessingCreate = true;
                var createdPatient = await FhirService.CreatePatientsAsync(patient);
                Patients.Add(createdPatient);
                ProcessingCreate = false;
                ToggleCreate();
                ShouldRender();
            }catch (Exception e)
            {
                Console.WriteLine("Exception");
                Console.WriteLine(e.Message);
            }
            
        }

        public void ToggleCreate()
        {
            ShowCreate = !ShowCreate;
        }
    }
}
