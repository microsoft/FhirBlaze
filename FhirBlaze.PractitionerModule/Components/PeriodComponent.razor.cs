using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;

namespace FhirBlaze.PractitionerModule.Components
{
    public partial class PeriodComponent
	{
		[Parameter]
		public Period Period { get; set; } = new Period();

		private DateTime PeriodStart
		{
			get
			{
				if (this.Period != null &&
					!string.IsNullOrWhiteSpace(this.Period.Start))
				{
					return DateTime.Parse(this.Period.Start);
				}
				else
				{
					return DateTime.MinValue;					
				}
			}
			set
			{
				if (this.Period == null)
				{
					this.Period = new Period();
				}
				this.Period.Start = value.ToString("yyyy-MM-dd");
			}
		}

		private DateTime PeriodEnd
		{
			get
			{
				if (this.Period != null &&
					!string.IsNullOrWhiteSpace(this.Period.End))
				{
					return DateTime.Parse(this.Period.End);
				}
				else
				{
					return DateTime.MinValue;					
				}
			}
			set
			{
				if (this.Period == null)
				{
					this.Period = new Period();
				}
				this.Period.End = value.ToString("yyyy-MM-dd");
			}
		}
	}
}
