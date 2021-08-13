using Bunit;
using FhirBlaze.PatientModule;
using Hl7.Fhir.Model;
using VerifyXunit;
using Xunit;
using Task = System.Threading.Tasks.Task;

[UsesVerify]
public class Tests :
    TestContext
{
    [Fact]
    public Task PatientDetailBinding()
    {
        var component = RenderComponent<PatientDetail>(builder =>
        {
            builder.Add(
                detail => detail.CurrentPatient,
                BuildPatient());
        });
        return Verifier.Verify(component);
    }

    static Patient BuildPatient()
    {
        return new()
        {
            Name = new()
            {
                new()
                {
                    Given = new[] { "John" },
                    Family = "Smith"
                }
            },
            Active = true,
            Gender = AdministrativeGender.Unknown,
            Telecom = new()
            {
                new(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Mobile, "1234")
            },
            BirthDate = "1/1/2020",
            Address = new()
            {
                new()
                {
                    City = "Seatle",
                    Country = "USA"
                }
            },
            Contact = new()
        };
    }
}