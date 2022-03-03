using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.MedicationModule.models
{
    public class SimpleMedication
    {
        public string MedicationID { get; set; }

        public Medication ToHl7Medication()
        {
            var Medication = new Medication();
            var MedicationIdentifier = new Identifier();

            MedicationIdentifier.System = "http://hlsemops.microsoft.com";
            MedicationIdentifier.Value = MedicationID;
            Medication.Identifier = new List<Hl7.Fhir.Model.Identifier>();
            Medication.Identifier.Add(MedicationIdentifier);
            Medication.Id = MedicationID;
            return Medication;
        }

        public Medication updateHL7FHIRMedication(Medication updateMedication)
        {
            var Medication = new Medication();
            var MedicationIdentifier = new Identifier();

            MedicationIdentifier.System = "http://hlsemops.microsoft.com";
            MedicationIdentifier.Value = MedicationID;
            Medication.Identifier = new List<Hl7.Fhir.Model.Identifier>();
            Medication.Identifier.Add(MedicationIdentifier);
            Medication.Id = MedicationID;
            updateMedication.Id = MedicationID;
            return updateMedication;
        }
    }
}
