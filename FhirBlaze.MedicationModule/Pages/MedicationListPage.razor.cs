using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.MedicationModule.Pages
{
    [Authorize]
    public partial class MedicationListPage
    {
        private bool _loading = true;

        [Inject]
        private IFhirService FhirService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public System.Threading.Tasks.Task<AuthenticationState> AuthTask { get; set; }

        private bool ShowSearch { get; set; } = false;

        public IList<Medication> Medications { get; set; } = new List<Medication>();

        protected override async Task OnInitializedAsync()
        {
            _loading = true;
            Medications = await FhirService.GetMedicationsAsync();
            _loading = false;
        }

        private void AddMedicationClicked()
        {
            NavigationManager.NavigateTo("/medication");
        }

        private void MedicationSelected(Medication medication)
        {
            NavigationManager.NavigateTo($"/medication/{medication.Id}");
        }

        private async Task SearchMedication(IDictionary<string, string> searchParameters)
        {
            try
            {
                _loading = true;
                this.Medications = await FhirService.SearchMedication(searchParameters);
                _loading = false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception");
                Console.WriteLine(e.Message); //TODO: manage the cancel search
            }
        }

    }
}
