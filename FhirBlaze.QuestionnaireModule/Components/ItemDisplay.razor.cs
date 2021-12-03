using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hl7.Fhir.Model.Questionnaire;

namespace FhirBlaze.QuestionnaireModule.Components
{
    public partial class ItemDisplay
    {
        [Parameter]
        public Questionnaire.ItemComponent ItemComponent { get; set; }

        protected string GetHeader(Questionnaire.ItemComponent item)
        {
            return (item.Type.Equals(Questionnaire.QuestionnaireItemType.Group)) ? "Group" : $"{item.Type.ToString()} Question";
        }
        protected string GetTitleText(Questionnaire.ItemComponent item)
        {
            return (item.Type.Equals(Questionnaire.QuestionnaireItemType.Group)) ? "Group Description" : "Question Text";
        }
        protected void AddItem(QuestionnaireItemType type)
        {
            var nItem = new Questionnaire.ItemComponent();
            nItem.Type = type;
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

        protected void OnUpClick(ItemComponent item)
        {
            MoveItem(ItemComponent.Item.IndexOf(item) - 1, item);
        }

        protected void OnDownClick(ItemComponent item)
        {
            MoveItem(ItemComponent.Item.IndexOf(item)+1,item);
           
        }

        protected void MoveItem(int NewIndex, ItemComponent item)
        {
            ItemComponent.Item.Remove(item);
            ItemComponent.Item.Insert(NewIndex, item);
        }

    }
}
