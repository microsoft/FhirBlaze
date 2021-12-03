using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hl7.Fhir.Model.Questionnaire;

namespace FhirBlaze.QuestionnaireModule.Components
{
    [Authorize]
    public partial class QuestionnaireComponent
    {
        //public Questionnaire Questionnaire { get; set; } = new Questionnaire();
        [Parameter]
        public Questionnaire Questionnaire { get; set; }
        [Parameter]
        public EventCallback<Questionnaire> SaveQuestionnaire { get; set; }


        protected override System.Threading.Tasks.Task OnInitializedAsync()
        {
            InitializeQuestionnaire();
            return base.OnInitializedAsync();
        }  
        
        protected async void InitializeQuestionnaire()
        {
            if (Questionnaire == null)
            {
                Questionnaire = new Questionnaire();

                var QuestionnaireIdentifier = new Identifier();
                QuestionnaireIdentifier.System = "http://hlsemops.microsoft.com";
                QuestionnaireIdentifier.Value = Guid.NewGuid().ToString();
                Questionnaire.Identifier = new List<Hl7.Fhir.Model.Identifier>();
                Questionnaire.Identifier.Add(QuestionnaireIdentifier);
                Questionnaire.Status = PublicationStatus.Draft;
                Questionnaire.Title = "New Questionnaire";
                Questionnaire.Description = new Markdown { Value = "A new questionnaire" };
            }
        }
        protected void AddItem(QuestionnaireItemType type)
        {
            var nItem = new Questionnaire.ItemComponent();
            nItem.Type = type;
            Questionnaire.Item.Add(nItem);
        }         

        protected void RemoveItem(Questionnaire.ItemComponent Item)
        {
            Questionnaire.Item.Remove(Item);
        }

        protected string GetHeader(Questionnaire.ItemComponent item)
        {
            return (item.Type.Equals(Questionnaire.QuestionnaireItemType.Group))?"Group": $"{item.Type.ToString()} Question";        
        }
        public async void Submit()
        {
            Questionnaire.Status = PublicationStatus.Draft;
            await SaveQuestionnaire.InvokeAsync(Questionnaire);
        }
    }
}
