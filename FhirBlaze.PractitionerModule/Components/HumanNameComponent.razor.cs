using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;

namespace FhirBlaze.PractitionerModule.Components
{
    public partial class HumanNameComponent
	{
		[Parameter]
		public HumanName HumanName { get; set; }

		private string Given
		{
			get => String.Join(',', this.HumanName.Given);
			set
			{
				this.HumanName.Given = value.Split(',');
			}
		}

		private string Prefix
		{
			get => String.Join(',', this.HumanName.Prefix);
			set
			{
				this.HumanName.Prefix = value.Split(',');
			}
		}

		private string Suffix
		{
			get => String.Join(',', this.HumanName.Suffix);
			set
			{
				this.HumanName.Suffix = value.Split(',');
			}
		}
	}
}
