using FhirBlaze.MedicationModule.models;
using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.MedicationModule
{
    [Authorize]
    public partial class MedicationList
    {
        [Inject]
        IFhirService FhirService { get; set; }
        protected bool ShowCreate { get; set; } = false;
        protected bool ShowUpdate { get; set; } = false;
        protected bool ShowSearch { get; set; } = false;
        protected bool Loading { get; set; } = true;

        protected bool ProcessingSearch { get; set; } = false;

        protected SimpleMedication DraftMedication { get; set; } = new SimpleMedication();

        protected Medication SelectedMedication { get; set; } = new Medication();
        [CascadingParameter] public Task<AuthenticationState> AuthTask { get; set; }
        public IList<Medication> Medications { get; set; } = new List<Medication>();

        protected override async Task OnInitializedAsync()
        {
            Loading = true;
            await base.OnInitializedAsync();
            Medications = await FhirService.GetMedicationsAsync();
            Loading = false;
            ShouldRender();
        }

        public async Task<Medication> CreateMedication(Medication medication)
        {
            ResetSelectedMedication();
            Medication createdMedication = null;
            try
            {
                createdMedication = await FhirService.CreateMedicationsAsync(medication);
                Medications.Add(createdMedication);
                ToggleCreate();
                ShouldRender();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception");
                Console.WriteLine(e.Message);
            }
            return createdMedication;
        }


        public async Task SearchMedication(Medication medication)
        {
            ResetSelectedMedication();
            try
            {
                Medications = await FhirService.SearchMedication(medication); //change to medication
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

        public async Task<Medication> UpdateMedication(Medication updatedMedication)
        {


            try
            {
                updatedMedication = await FhirService.UpdateMedicationAsync(updatedMedication.Id, updatedMedication);
                var removeMedication = Medications.FirstOrDefault(p => p.Id == SelectedMedication.Id);
                if (removeMedication != null)
                {
                    Medications.Remove(removeMedication);
                    Medications.Add(updatedMedication);
                }
                SelectedMedication = updatedMedication;
                ToggleUpdate();
                ShouldRender();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured editing medication");
                Console.WriteLine(e.Message);
            }
            return updatedMedication;
        }

        public void ToggleCreate()
        {
            ShowCreate = !ShowCreate;
            ResetSelectedMedication();
        }

        public void ToggleUpdate()
        {
            ShowUpdate = !ShowUpdate;
        }

        public void ToggleSearch()
        {
            ShowSearch = !ShowSearch;
            ResetSelectedMedication();
            if (ShowSearch)
            {
                DraftMedication = new SimpleMedication();
            }
        }

        private void ResetSelectedMedication()
        {
            SelectedMedication = null;
        }

        private void MedicationSelected(EventArgs e, Medication newMedication)
        {
            SelectedMedication = newMedication;
            ToggleUpdate();
        }
    }
}
