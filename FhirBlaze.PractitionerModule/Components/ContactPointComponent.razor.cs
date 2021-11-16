using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using static Hl7.Fhir.Model.ContactPoint;

namespace FhirBlaze.PractitionerModule.Components
{
    public partial class ContactPointComponent
    {
        [Parameter]
        public ContactPoint ContactPoint { get; set; }

		public string Use
        {
            get
            {
				if (this.ContactPoint.Use == null)
                {
					return string.Empty;
                }
                else
				{
					return this.ContactPoint.Use.Value.ToString();
				}
            }
            set
            {
				this.ContactPoint.Use = Enum.Parse<ContactPointUse>(value, true);
            }
        }

		public string System
        {
            get
            {
				if (this.ContactPoint.System == null)
                {
					return string.Empty;
                }
                else
				{
					return this.ContactPoint.System.Value.ToString();
				}
            }
            set
            {
				this.ContactPoint.System = Enum.Parse<ContactPointSystem>(value, true);
            }
        }

		private DateTime PeriodStart
		{
			get
			{
				if (this.ContactPoint != null &&
					this.ContactPoint.Period != null &&
					!string.IsNullOrWhiteSpace(this.ContactPoint.Period.Start))
				{
					return DateTime.Parse(this.ContactPoint.Period.Start);
				}
				else
				{
					return DateTime.MinValue;
				}
			}
			set
			{
				if (this.ContactPoint.Period == null)
				{
					this.ContactPoint.Period = new Period();
				}
				this.ContactPoint.Period.Start = value.ToString("yyyy-MM-dd");
			}
		}

		private DateTime PeriodEnd
		{
			get
			{
				if (this.ContactPoint != null &&
					this.ContactPoint.Period != null &&
					!string.IsNullOrWhiteSpace(this.ContactPoint.Period.End))
				{
					return DateTime.Parse(this.ContactPoint.Period.End);
				}
				else
				{
					return DateTime.MinValue;
				}
			}
			set
			{
				if (this.ContactPoint.Period == null)
                {
					this.ContactPoint.Period = new Period();
                }
				this.ContactPoint.Period.End = value.ToString("yyyy-MM-dd");
			}
		}
	}
}
