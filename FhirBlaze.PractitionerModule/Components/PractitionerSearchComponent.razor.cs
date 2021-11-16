using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace FhirBlaze.PractitionerModule.Components
{
    public partial class PractitionerSearchComponent
    {
        private IDictionary<string, string> SearchParameters { get; set; }

        [Parameter]
        public EventCallback<IDictionary<string, string>> SearchClicked { get; set; }

        protected override void OnInitialized()
        {
            this.SearchParameters = new Dictionary<string, string>();
            this.SearchParameters["identifier"] = string.Empty;
            this.SearchParameters["given"] = string.Empty;
            this.SearchParameters["family"] = string.Empty;
        }

        protected async void SearchPractitioner()
        {
            await SearchClicked.InvokeAsync(this.SearchParameters);
        }
    }
}
