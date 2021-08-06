using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.QuestionnaireModule
{
    [Authorize]
    public partial class CreateQuestionnaire
    {
        public Questionnaire Questionnaire { get; set; } = new Questionnaire();

        protected Questionnaire.ItemComponent ItemComponenet { get; set; }

        protected IList<ItemDisplay> ItemDisplays { get; set; }

        protected IList<Questionnaire.ItemComponent> NewQuestionnaireItems { get; set; } = new List<Questionnaire.ItemComponent>();

        public int BedResponse { get; set; }
        public int TVResponse { get; set; }
        public int ACResponse { get; set; }
        public int BathroomResponse { get; set; }

        protected async void SaveQuestionnaire()
        {

        }

        protected void AddItem()
        {
            ItemDisplays.Add(new ItemDisplay());
        }

        protected void AddStringItem()
        {
            NewQuestionnaireItems.Add(new Questionnaire.ItemComponent()
            {
                LinkId = new Guid().ToString(),
                Type = Questionnaire.QuestionnaireItemType.String,
                Text = ""
            });
        }
        protected void AddBooleanItem()
        {
            NewQuestionnaireItems.Add(new Questionnaire.ItemComponent()
            {
                LinkId = new Guid().ToString(),
                Type = Questionnaire.QuestionnaireItemType.Boolean,
                Text = ""
            });
        }

        protected void AddGroupItem()
        {
            NewQuestionnaireItems.Add(new Questionnaire.ItemComponent()
            {
                LinkId = new Guid().ToString(),
                Type = Questionnaire.QuestionnaireItemType.Group,
                Text = ""
            });
        }

        protected void AddBooleanQuestion()
        {

        }
    }
}
