using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace FhirBlaze.PractitionerModule.Components
{
    public partial class PractitionerListComponent
    {
        [Parameter]
        public EventCallback<Practitioner> OnPractitionerSelected { get; set; }

        [CascadingParameter]
        public IList<Practitioner> Practitioners { get; set; } = new List<Practitioner>();

        private void PractitionerSelected(Practitioner practitioner)
        {
            OnPractitionerSelected.InvokeAsync(practitioner);
        }
    }
}
