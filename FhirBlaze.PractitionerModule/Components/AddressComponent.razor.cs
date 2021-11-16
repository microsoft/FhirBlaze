using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using static Hl7.Fhir.Model.Address;

namespace FhirBlaze.PractitionerModule.Components
{
    public partial class AddressComponent
    {
        [Parameter]
        public Address Address { get; set; }

		private string Use
        {
            get
            {
				if (this.Address.Use == null)
                {
					return string.Empty;
                }
                else
				{
					return this.Address.Use.Value.ToString();
				}
            }
            set
            {
				this.Address.Use = Enum.Parse<AddressUse>(value, true);
            }
        }

		private string Type
        {
            get
            {
				if (this.Address.Type == null)
                {
					return string.Empty;
                }
                else
				{
					return this.Address.Type.Value.ToString();
				}
            }
            set
            {
				this.Address.Type = Enum.Parse<AddressType>(value, true);
            }
        }

		private string Line
		{
			get => String.Join(',', this.Address.Line);
			set
			{
				this.Address.Line = value.Split(',');
			}
		}

		private DateTime PeriodStart
		{
			get
			{
				if (this.Address != null &&
					this.Address.Period != null &&
					!string.IsNullOrWhiteSpace(this.Address.Period.Start))
				{
					return DateTime.Parse(this.Address.Period.Start);
				}
				else
				{
					return DateTime.MinValue;
				}
			}
			set
			{
				if (this.Address.Period == null)
				{
					this.Address.Period = new Period();
				}
				this.Address.Period.Start = value.ToString("yyyy-MM-dd");
			}
		}

		private DateTime PeriodEnd
		{
			get
			{
				if (this.Address != null &&
					this.Address.Period != null &&
					!string.IsNullOrWhiteSpace(this.Address.Period.End))
				{
					return DateTime.Parse(this.Address.Period.End);
				}
				else
				{
					return DateTime.MinValue;
				}
			}
			set
			{
				if (this.Address.Period == null)
				{
					this.Address.Period = new Period();
				}
				this.Address.Period.End = value.ToString("yyyy-MM-dd");
			}
		}
	}
}
