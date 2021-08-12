using FhirBlaze.PatientModule.models;
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
        protected bool ShowSearch { get; set; } = false;
        protected bool Loading { get; set; } = true;
        protected bool ProcessingCreate { get; set; } = false;
        protected bool ProcessingSearch { get; set; } = false;
        protected SimplePatient DraftPatient {get;set;}


        public IList<Patient> Patients { get; set; } = new List<Patient>();
        public IList<Patient> PatientsSearched { get; set; } = new List<Patient>();

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {            
            Loading = true;
            await base.OnInitializedAsync();
            Patients = await FhirService.GetPatientsAsync();
            Loading = false;
            ShouldRender();
        }

        public async Task<Patient> CreatePatient(Patient patient)
        {
            Patient createdPatient = null;
            try
            {
                ProcessingCreate = true;
                createdPatient = await FhirService.CreatePatientsAsync(patient);
                Patients.Add(createdPatient);
                ProcessingCreate = false;
                ToggleCreate();
                ShouldRender();
            }catch (Exception e)
            {
                Console.WriteLine("Exception");
                Console.WriteLine(e.Message);
            }
            return createdPatient;
        }


        public async Task<Patient> SearchPatient(Patient patient)
        {

            Patient createdPatient = null;
            try
            {
                //var testy = await FhirService.SearchPatient();
                Patients = await FhirService.SearchPatient(patient); //change to patient
                ProcessingSearch = true;
                
                //Patients.Add(createdPatient);
                ProcessingSearch = false;
                ToggleSearch();
                ShouldRender();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception");
                Console.WriteLine(e.Message); //manage the cancel search
            }
            return createdPatient;
        }

        public void ToggleCreate()
        {
            ShowCreate = !ShowCreate;
            if (ShowCreate)
            {
                DraftPatient = new SimplePatient() {
                    PatientID = Guid.NewGuid().ToString(),
                    Birthdate = DateTime.Now.AddDays(DateTime.Now.Second).AddMonths(DateTime.Now.Hour).AddYears(-DateTime.Now.Second)
                };
            }


        }


        public void ToggleSearch()
        {
            ShowSearch = !ShowSearch;
            if (ShowSearch)
            {
                DraftPatient = new SimplePatient()
                {
                    //PatientID = Guid.NewGuid().ToString(),
                    //Birthdate = DateTime.Now.AddDays(DateTime.Now.Second).AddMonths(DateTime.Now.Hour).AddYears(-DateTime.Now.Second)
                };
            }


            
        }
    }
}
