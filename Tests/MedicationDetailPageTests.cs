using FhirBlaze.MedicationModule.Pages;
using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Xunit;

namespace MedicationModule
{
    public class MedicationDetailPageTests
    {
        [Fact]
        public void NotNullTests()
        {
            var page = new MedicationDetailPage();

            Assert.NotNull(page.MedicationCodes);
        }

        [Fact]
        public void ValueSetCodeTypeTests()
        {
            var page = new MedicationDetailPage();

            Assert.IsType<ValueSet>(page.MedicationCodes);
        }
    }
}