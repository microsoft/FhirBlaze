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
                Practitioner = new Practitioner()
            };
            component.AddHumanName();

            Assert.Collection<HumanName>(
                component.Practitioner.Name,
                item => new HumanName());

            component.AddHumanName();

            Assert.Collection<HumanName>(
                component.Practitioner.Name,
                item => new HumanName(), 
                item => new HumanName());
        }
    }
}
