using FhirBlaze.MedicationModule.Components;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Xunit;

namespace MedicationModule
{
    public class MedicationListComponentTests
    {
        [Fact]
        public void NotNullTests()
        {
            var component = new MedicationListComponent();

            foreach (Medication med in component.Medications)
            {
              foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(med))
              {
                string name = descriptor.Name;
                object value = descriptor.GetValue(med);
                Console.WriteLine("{0}={1}", name, value);
              }
            }

            Assert.NotNull(component.Medications);
        }

        [Fact]
        public void MedicationTypeTests()
        {
            var component = new MedicationListComponent()
            {
              Medications = new List<Medication>()
            };

            Assert.IsType<List<Medication>>(component.Medications);
        }

        [Fact]
        public void MedicationCountTests()
        {
            var component = new MedicationListComponent()
            {
              Medications = new List<Medication>()
            };

            Assert.Equal(0, component.Medications.Count);
        }

        [Fact]
        public void MedicationPropertiesTests()
        {
            var component = new MedicationListComponent()
            {
              Medications = new List<Medication>(){
                new Medication()
                {
                  Text = new Narrative()
                  {
                    Status = new Narrative.NarrativeStatus(),
                    Div = ""
                  }
                }
              }
            };

            Assert.True(hasTextProperty(component.Medications[0]));
            Assert.True(textHasDivProperty(component.Medications[0]));
            Assert.True(textHasStatusProperty(component.Medications[0]));
        }

        private bool hasTextProperty(Medication medication)
        {
          return medication.Text != null;
        }

        private bool textHasDivProperty(Medication medication)
        {
          return medication.Text.Div != null;
        }

        private bool textHasStatusProperty(Medication medication)
        {
          return medication.Text.Status != null;
        }
    }
}
