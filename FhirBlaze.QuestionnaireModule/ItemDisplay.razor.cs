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
        public Questionnaire.ItemComponent ItemComponent { get; set; }

        [Parameter]
        public bool Edit { get; set; } = false;

        [Parameter]
        public EventCallback<Questionnaire.ItemComponent> OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback<Questionnaire.ItemComponent> CancelItemCreation { get; set; }

        [Parameter]
        public EventCallback<string> ItemComponentTextChanged { get; set; }

        [Parameter]
        public EventCallback<Questionnaire.ItemComponent> ItemComponentChanged { get; set; }

        private async System.Threading.Tasks.Task ModifyItemComponent() => await ItemComponentChanged.InvokeAsync(ItemComponent);

        private void GetOption(ChangeEventArgs e)
        {
            int type = Int32.Parse(e.Value.ToString());
            ItemComponent.Type = (Hl7.Fhir.Model.Questionnaire.QuestionnaireItemType)(type);
        }

    }
}
