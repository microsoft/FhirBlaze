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
        public Practitioner SelectedPractitioner { get; set; }

        public bool Active
        {
            get 
            {
                if (this.SelectedPractitioner.Active == null || this.SelectedPractitioner.Active == false)
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
                SelectedPractitioner.Active = value; 
            }
        }

        public string Gender
        {
            get
            {
                if (this.SelectedPractitioner!= null && this.SelectedPractitioner.Gender != null)
                {
                    return this.SelectedPractitioner.Gender.Value.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                this.SelectedPractitioner.Gender = Enum.Parse<AdministrativeGender>(value, true);
            }
        }

        private DateTime BirthDate
        {
            get
            {
                if (this.SelectedPractitioner != null &&
                    !string.IsNullOrWhiteSpace(this.SelectedPractitioner.BirthDate))
                {
                    return DateTime.Parse(this.SelectedPractitioner.BirthDate);
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            set => this.SelectedPractitioner.BirthDate = value.ToString("yyyy-MM-dd");
        }

        protected async void SavePractitioner()
        {
            await OnPractitionerSaved.InvokeAsync(this.SelectedPractitioner);
        }

        public void AddHumanName()
        {
            if (this.SelectedPractitioner.Name == null)
            {
                this.SelectedPractitioner.Name = new List<HumanName>();
            }
            this.SelectedPractitioner.Name.Add(new HumanName() { });
        }

        private void RemoveHumanName(HumanName name)
        {
            this.SelectedPractitioner.Name.Remove(name);
        }

        private void AddAddress()
        {
            if(this.SelectedPractitioner.Address == null)
            {
                this.SelectedPractitioner.Address = new List<Address>();
            }
            this.SelectedPractitioner.Address.Add(new Address());
        }

        private void RemoveAddress(Address address)
        {
            this.SelectedPractitioner.Address.Remove(address);
        }

        private void AddTelecom()
        {
            if (this.SelectedPractitioner.Telecom == null)
            {
                this.SelectedPractitioner.Telecom = new List<ContactPoint>();
            }
            this.SelectedPractitioner.Telecom.Add(new ContactPoint());
        }

        private void RemoveTelecom(ContactPoint telecom)
        {
            this.SelectedPractitioner.Telecom.Remove(telecom);
        }
    }
}
