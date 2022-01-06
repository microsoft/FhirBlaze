using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
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
                if (this.SelectedPractitioner != null) { 
                    if (this.SelectedPractitioner.Address != null) {
                        foreach (var x in this.SelectedPractitioner.Address)
                        {
                            if (x.Period == null) { x.Period = new Period(); }
                        }                
                    }
                    if (this.SelectedPractitioner.Name != null)
                    {
                        foreach (var x in this.SelectedPractitioner.Name)
                        {
                            if (x.Period == null) { x.Period = new Period(); }
                        }
                    }
                    if (this.SelectedPractitioner.Telecom != null)
                    {
                        foreach (var x in this.SelectedPractitioner.Telecom)
                        {
                            if (x.Period == null) { x.Period = new Period(); }
                        }
                    }
                }
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
