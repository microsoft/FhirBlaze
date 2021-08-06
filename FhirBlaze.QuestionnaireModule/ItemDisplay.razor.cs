using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.QuestionnaireModule
{
    public partial class ItemDisplay
    {
        [Parameter]
        public Questionnaire.ItemComponent ItemComponenet { get; set; }

        [Parameter]
        public bool Edit { get; set; } = false;

        [Parameter]
        public EventCallback<Questionnaire.ItemComponent> OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback<Questionnaire.ItemComponent> CancelItemCreation { get; set; }
    }
}
