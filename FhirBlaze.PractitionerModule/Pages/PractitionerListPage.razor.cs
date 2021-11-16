using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.PractitionerModule.Pages
{
    [Authorize]
    public partial class PractitionerListPage
    {
        private bool _loading = true;

        [Inject]
        private IFhirService FhirService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public System.Threading.Tasks.Task<AuthenticationState> AuthTask { get; set; }

        private bool ShowSearch { get; set; } = false;

        public IList<Practitioner> Practitioners { get; set; } = new List<Practitioner>();

        protected override async Task OnInitializedAsync()
        {
            _loading = true;
            Practitioners = await FhirService.GetPractitionersAsync();
            _loading = false;
        }

        private void AddPractitionerClicked()
        {
            NavigationManager.NavigateTo("/practitioner");
        }

        private void PractitionerSelected(Practitioner practitioner)
        {
            NavigationManager.NavigateTo($"/practitioner/{practitioner.Id}");
        }

        private async Task SearchPractitioner(IDictionary<string, string> searchParameters)
        {
            try
            {
                _loading = true;
                this.Practitioners = await FhirService.SearchPractitioner(searchParameters);
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
