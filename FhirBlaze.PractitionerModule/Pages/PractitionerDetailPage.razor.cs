using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.PractitionerModule.Pages
{
    [Authorize]
    public partial class PractitionerDetailPage
	{
		[Inject]
		private IFhirService FhirService { get; set; }

		[Parameter]
		public string Id { get; set; }

        private Practitioner SelectedPractitioner { get; set; } = new Practitioner();

		protected override async Task OnParametersSetAsync()
		{
			if (this.Id != null)
			{
				this.SelectedPractitioner = await FhirService.GetResourceByIdAsync<Practitioner>(this.Id);
			}
			else
			{
				this.SelectedPractitioner = new Practitioner() { Active = true };
			}
        }

        private async Task SavePractitioner(Practitioner practitioner)
        {
            Practitioner persistedPractitioner = new Practitioner();
            try
            {
                if (string.IsNullOrEmpty(practitioner.Id))
                {
                    persistedPractitioner = await FhirService.CreatePractitionersAsync(practitioner);
                }
                else
                {
                    persistedPractitioner = await FhirService.UpdatePractitionerAsync(practitioner.Id, practitioner);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            this.SelectedPractitioner = persistedPractitioner;
        }
    }
}
