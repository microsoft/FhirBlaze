using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace FhirBlaze.PractitionerModule.Components
{
    public partial class PractitionerDetailComponent
    {
        [Parameter]
        public EventCallback<Practitioner> OnPractitionerSaved { get; set; }

        [Parameter]
        public bool Processing { get; set; }

        [Parameter]
        public bool IsEditable { get; set; } = true;

        [CascadingParameter]
        public Practitioner Practitioner { get; set; }

        public bool Active
        {
            get 
            {
                if (this.Practitioner.Active == null || this.Practitioner.Active == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            { 
                Practitioner.Active = value; 
            }
        }

        public string Gender
        {
            get
            {
                if (this.Practitioner!= null && this.Practitioner.Gender != null)
                {
                    return this.Practitioner.Gender.Value.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                this.Practitioner.Gender = Enum.Parse<AdministrativeGender>(value, true);
            }
        }

        private DateTime BirthDate
        {
            get
            {
                if (this.Practitioner != null &&
                    !string.IsNullOrWhiteSpace(this.Practitioner.BirthDate))
                {
                    return DateTime.Parse(this.Practitioner.BirthDate);
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            set => this.Practitioner.BirthDate = value.ToString("yyyy-MM-dd");
        }

        protected async void SavePractitioner()
        {
            await OnPractitionerSaved.InvokeAsync(this.Practitioner);
        }

        public void AddHumanName()
        {
            if (this.Practitioner.Name == null)
            {
                this.Practitioner.Name = new List<HumanName>();
            }
            this.Practitioner.Name.Add( new HumanName() { Period = new Period() });
        }

        private void RemoveHumanName(HumanName name)
        {
            this.Practitioner.Name.Remove(name);
        }

        private void AddAddress()
        {
            if(this.Practitioner.Address == null)
            {
                this.Practitioner.Address = new List<Address>();
            }            
            this.Practitioner.Address.Add(new Address() { Period = new Period() });
        }

        private void RemoveAddress(Address address)
        {
            this.Practitioner.Address.Remove(address);
        }

        private void AddTelecom()
        {
            if (this.Practitioner.Telecom == null)
            {
                this.Practitioner.Telecom = new List<ContactPoint>();
            }
            this.Practitioner.Telecom.Add(new ContactPoint() { Period = new Period() });
        }

        private void RemoveTelecom(ContactPoint telecom)
        {
            this.Practitioner.Telecom.Remove(telecom);
        }
    }
}
