using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hl7.Fhir.Model.Questionnaire;

namespace FhirBlaze.QuestionnaireModule
{
    public partial class ItemDisplay
    {
        [Parameter]
        public Questionnaire.ItemComponent ItemComponent { get; set; }
        private string Header { get; set; }
        private string TextDesc { get; set; }

        protected override void OnInitialized()
        {
            if (ItemComponent.Type.Equals(Questionnaire.QuestionnaireItemType.Group))
            {
                Header = "Group";
                TextDesc = "Group Description";                     
            }
            else
            {
                Header = $"{ItemComponent.Type.ToString()} Question";
                TextDesc = "Question Text";
            }
            base.OnInitialized();
        }
        protected void AddItem(QuestionnaireItemType type)
        {
            var nItem = new Questionnaire.ItemComponent();
            nItem.Type = type;
            switch (type)
            {
                case Questionnaire.QuestionnaireItemType.Group:
                    nItem.Text = "New Group";
                    break;
                case Questionnaire.QuestionnaireItemType.String:
                    nItem.Text = "String Question";
                    break;
                default:
                    break;
            }
            ItemComponent.Item.Add(nItem);
        }
        protected void RemoveItem(Questionnaire.ItemComponent Item)
        {
            ItemComponent.Item.Remove(Item);
        }

        protected void AddAnswer()
        {
            var nCode = new Coding();
            nCode.Display = "Display";
            nCode.Code = "Value";
            var a = new Questionnaire.AnswerOptionComponent();
            a.Value=nCode;
            ItemComponent.AnswerOption.Add(a);
        }

    }
}
