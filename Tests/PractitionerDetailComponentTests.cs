using FhirBlaze.PractitionerModule.Components;
using Hl7.Fhir.Model;
using Xunit;

namespace PractitionerModule
{
    public class PractitionerDetailComponentTests
    {
        [Fact]
        public void AddHumanNameTests()
        {
            var component = new PractitionerDetailComponent()
            {
                SelectedPractitioner = new Practitioner()
            };
            component.AddHumanName();

            Assert.Collection<HumanName>(
                component.SelectedPractitioner.Name,
                item => new HumanName());

            component.AddHumanName();

            Assert.Collection<HumanName>(
                component.SelectedPractitioner.Name,
                item => new HumanName(), 
                item => new HumanName());
        }
    }
}
