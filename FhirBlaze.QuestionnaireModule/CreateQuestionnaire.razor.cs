using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.QuestionnaireModule
{
    public partial class CreateQuestionnaire
    {
        public Questionnaire Questionnaire { get; set; } = new Questionnaire();

        public HumanName PatientName { get; set; } = new HumanName();
       
        public int BedResponse { get; set; }
        public int TVResponse { get; set; }
        public int ACResponse { get; set; }
        public int BathroomResponse { get; set; }

        protected void CreatePatient()
        {
            
        }
    }
}
