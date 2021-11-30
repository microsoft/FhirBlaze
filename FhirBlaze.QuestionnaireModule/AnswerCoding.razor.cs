using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.QuestionnaireModule
{
    public partial class AnswerCoding
    {
        [Parameter]
        public Coding Coding { get; set; }
    }
}
