using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace FhirBlaze.MedicationModule.Components
{
    public partial class MedicationSearchComponent
    {
        private IDictionary<string, string> SearchParameters { get; set; }

        [Parameter]
        public EventCallback<IDictionary<string, string>> SearchClicked { get; set; }

        protected override void OnInitialized()
        {
            this.SearchParameters = new Dictionary<string, string>();
            this.SearchParameters["identifier"] = string.Empty;
            this.SearchParameters["identifier"] = string.Empty;
        }

        protected async void SearchMedication()
        {
            await SearchClicked.InvokeAsync(this.SearchParameters);
        }
    }
}
