using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
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
        [Inject]
        IFhirService FhirService { get; set; }
        public Questionnaire Questionnaire { get; set; } = new Questionnaire();

        protected Questionnaire.ItemComponent ItemComponent { get; set; }

        protected IList<ItemDisplay> ItemDisplays { get; set; }

        protected IList<Questionnaire.ItemComponent> NewQuestionnaireItems { get; set; } = new List<Questionnaire.ItemComponent>();

        protected async void InitializeQuestionnaire()
        {
            var QuestionnaireIdentifier = new Hl7.Fhir.Model.Identifier();
            QuestionnaireIdentifier.System = "http://hlsemops.microsoft.com";
            QuestionnaireIdentifier.Value = Guid.NewGuid().ToString();
            Questionnaire.Identifier = new List<Hl7.Fhir.Model.Identifier>();
            Questionnaire.Identifier.Add(QuestionnaireIdentifier);
            Questionnaire.Status = PublicationStatus.Draft;
        }

        public async void Submit()
        {
            //InitializeQuestionnaire();
            
            Questionnaire.Status = PublicationStatus.Draft;
            
            foreach (var item in NewQuestionnaireItems)
            {
                Questionnaire.Item.Add(item);
            }

            await FhirService.CreateQuestionnaireAsync(Questionnaire);
        }
    }
}
