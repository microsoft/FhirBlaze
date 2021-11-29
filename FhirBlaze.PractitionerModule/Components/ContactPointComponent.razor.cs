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
	}
}
