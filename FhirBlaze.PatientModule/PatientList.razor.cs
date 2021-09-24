using FhirBlaze.PatientModule.models;
using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.PatientModule
{
    [Authorize]
    public partial class PatientList
    {
        [Inject]
        IFhirService FhirService { get; set; }
        protected bool ShowCreate { get; set; } = false;
        protected bool ShowUpdate { get; set; } = false;
        protected bool ShowSearch { get; set; } = false;
        protected bool Loading { get; set; } = true;
        protected bool ProcessingCreate { get; set; } = false;
        protected bool ProcessingSearch { get; set; } = false;
        protected bool ProcessingUpdate { get; set; } = false;
        protected SimplePatient DraftPatient { get; set; } = new SimplePatient();
        protected SimplePatient EditPatient { get; set; } = new SimplePatient();
        protected Patient SelectedPatient { get; set; } = new Patient();

        public IList<Patient> Patients { get; set; } = new List<Patient>();

        protected override async Task OnInitializedAsync()
        {            
            Loading = true;
            await base.OnInitializedAsync();
            Patients = await FhirService.GetPatientsAsync();
            Loading = false;
            ShouldRender();
        }

        public async Task<Patient> CreatePatient(Patient patient)
        {
            ResetSelectedPatient();
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


        public async Task SearchPatient(Patient patient)
        {
            ResetSelectedPatient();
            try
            {
                Patients = await FhirService.SearchPatient(patient); //change to patient
                ProcessingSearch = true;
                
                ProcessingSearch = false;
                ToggleSearch();
                ShouldRender();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception");
                Console.WriteLine(e.Message); //manage the cancel search
            }
        }

        public async Task<Patient> UpdatePatient(SimplePatient patient)
        {
            Patient updatedPatient = patient.updateHL7FHIRPatient(SelectedPatient);
            
            try
            {
                ProcessingUpdate = true;
                updatedPatient = await FhirService.UpdatePatientAsync(updatedPatient.Id, updatedPatient);
                var removePatient = Patients.FirstOrDefault(p => p.Id == SelectedPatient.Id);
                if (removePatient != null)
                {
                    Patients.Remove(removePatient);
                    Patients.Add(updatedPatient);
                }
                SelectedPatient = updatedPatient;
                ProcessingUpdate = false;
                ToggleUpdate();
                ShouldRender();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured editing patient");
                Console.WriteLine(e.Message);
            }
            return updatedPatient;
        }

        public void ToggleCreate()
        {
            ShowCreate = !ShowCreate;
            ResetSelectedPatient();
            if (ShowCreate)
            {
                DraftPatient = new SimplePatient {
                    PatientID = Guid.NewGuid().ToString(),
                    Birthdate = DateTime.Now.AddDays(DateTime.Now.Second).AddMonths(DateTime.Now.Hour).AddYears(-DateTime.Now.Second)
                };
            }
        }

        public void ToggleUpdate()
        {   
            ShowUpdate = !ShowUpdate;
            if (ShowUpdate)
            {
                var name = SelectedPatient.Name.FirstOrDefault();

                EditPatient = new SimplePatient
                {
                    PatientID = SelectedPatient.Id,
                    FirstName = name?.Given.FirstOrDefault(),
                    LastName = name?.Family,
                    Birthdate = DateTime.Parse(SelectedPatient.BirthDate)
                };
            }
        }

        public void ToggleSearch()
        {
            ShowSearch = !ShowSearch;
            ResetSelectedPatient();
            if (ShowSearch)
            {
                DraftPatient = new SimplePatient();
            }
        }

        private void ResetSelectedPatient()
        {
            SelectedPatient = null;
        }

        private void PatientSelected(EventArgs e, Patient newPatient)
        {
            SelectedPatient = newPatient;
            ToggleUpdate();
        }
    }
}
