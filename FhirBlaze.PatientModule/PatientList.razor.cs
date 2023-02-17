using FhirBlaze.PatientModule.models;
using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Graph;
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
        protected bool ShowNaturalLanguageSearch { get; set; } = false;
        protected bool Loading { get; set; } = true;
        
        protected bool ProcessingSearch { get; set; } = false;
        
        protected bool ShowQuestionnaireList { get; set; } = false;
        protected string QuestionnaireId { get; set; } = null;
        protected SimplePatient DraftPatient { get; set; } = new SimplePatient();
        
        protected Patient SelectedPatient { get; set; } = new Patient();
        [CascadingParameter] public Task<AuthenticationState> AuthTask { get; set; }
        public IList<Patient> Patients { get; set; } = new List<Patient>();

        protected override async Task OnInitializedAsync()
        {
            await FetchPatientsAsync();
        }

        public async Task FetchPatientsAsync()
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
                createdPatient = await FhirService.CreatePatientsAsync(patient);
                Patients.Add(createdPatient);
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

        public async Task<Patient> UpdatePatient(Patient updatedPatient)
        {
            try
            {
                updatedPatient = await FhirService.UpdatePatientAsync(updatedPatient.Id, updatedPatient);
                var removePatient = Patients.FirstOrDefault(p => p.Id == SelectedPatient.Id);
                if (removePatient != null)
                {
                    Patients.Remove(removePatient);
                    Patients.Add(updatedPatient);
                }
                SelectedPatient = updatedPatient;
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

        public async Task UpdatePatientListAsync(List<Patient> patients)
        {
            Patients = patients;
        }

        public void ToggleCreate()
        {
            ShowCreate = !ShowCreate;
            ResetSelectedPatient();
        }

        public void ToggleUpdate()
        {   
            ShowUpdate = !ShowUpdate; 
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

        public void ToggleNaturalLanguageSearch()
        {
            ShowNaturalLanguageSearch = !ShowNaturalLanguageSearch;
            ResetSelectedPatient();
            if (ShowNaturalLanguageSearch)
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

        public void ToggleQList(string qID)
        {
            QuestionnaireId = qID;
            ShowQuestionnaireList = !ShowQuestionnaireList;
        }
    }
}
