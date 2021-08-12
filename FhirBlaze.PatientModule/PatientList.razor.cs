using FhirBlaze.PatientModule.models;
using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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

        public async Task<Patient> UpdatePatient(Patient patient)
        {
            Patient updatedPatient = patient;
            try
            {
                ProcessingUpdate = true;
                updatedPatient = await FhirService.UpdatePatientAsync(patient);
                ProcessingUpdate = false;
                ToggleUpdate();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured editing patient");
                Console.WriteLine(e.Message);
                throw;
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
            string patientFirstName = "";
            string patientLastName = "";

            ShowUpdate = !ShowUpdate;
            if (ShowUpdate)
            {
                /*
                foreach (var identifier in SelectedPatient.Identifier)
                {
                    if (identifier.Value != "")
                    {
                        patientIdentifier = identifier.Value;
                        break;
                    }
                }
                */

                foreach (var name in SelectedPatient.Name)
                {
                    foreach (var givenName in name.GivenElement)
                    {
                        if (givenName.Value != "")
                        {
                            patientFirstName = givenName.Value;
                            break;
                        }
                    }

                    if (patientFirstName != "")
                    {
                        patientLastName = name.Family;
                        break;
                    }
                }

                EditPatient = new SimplePatient
                {
                    PatientID = SelectedPatient.Id,
                    FirstName = patientFirstName,
                    LastName = patientLastName
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
        }
    }
}
