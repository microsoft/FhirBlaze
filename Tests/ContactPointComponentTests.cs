using FhirBlaze.PractitionerModule.Components;
using Hl7.Fhir.Model;
using Xunit;

namespace PractitionerModule
{
    public class ContactPointComponentTests
    {
        [Fact]
        private void ContactPointUse()
        {
            var component = new ContactPointComponent()
            {
                ContactPoint = new ContactPoint()
                { Use = ContactPoint.ContactPointUse.Home }
            };

            Assert.Equal("Home", component.Use);
            component.Use = "Mobile";

            Assert.Equal("Mobile", component.Use);
        }

        [Fact]
        private void ContactPointSystem()
        {
            var component = new ContactPointComponent()
            {
                ContactPoint = new ContactPoint()
                { System = ContactPoint.ContactPointSystem.Pager }
            };

            Assert.Equal("Pager", component.System);
            component.System = "Phone";

            Assert.Equal("Phone", component.System);
        }
    }
}
