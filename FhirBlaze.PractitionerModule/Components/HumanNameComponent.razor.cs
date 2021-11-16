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

		private DateTime PeriodStart
		{
			get
			{
				if (this.HumanName != null && 
					this.HumanName.Period != null && 
					!string.IsNullOrWhiteSpace(this.HumanName.Period.Start))
				{
					return DateTime.Parse(this.HumanName.Period.Start);
                }
                else
				{
					return DateTime.MinValue;
				}
			}
			set
			{
				if (this.HumanName.Period == null)
                {
					this.HumanName.Period = new Period();
                }
				this.HumanName.Period.Start = value.ToString("yyyy-MM-dd");
			}
		}

		private DateTime PeriodEnd
		{
			get
			{
				if (this.HumanName != null && 
					this.HumanName.Period != null && 
					!string.IsNullOrWhiteSpace(this.HumanName.Period.End))
				{
					return DateTime.Parse(this.HumanName.Period.End);
				}
				else
				{
					if (this.HumanName.Period == null)
					{
						this.HumanName.Period = new Period();
					}
					return DateTime.MinValue;
				}
			}
			set => this.HumanName.Period.End = value.ToString("yyyy-MM-dd");
		}
	}
}
