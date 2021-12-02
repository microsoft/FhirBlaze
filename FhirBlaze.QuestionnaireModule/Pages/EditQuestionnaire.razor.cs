using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Task = System.Threading.Tasks.Task;
using static Hl7.Fhir.Model.Questionnaire;

namespace FhirBlaze.QuestionnaireModule.Pages
{
    [Authorize]
    public partial class EditQuestionnaire
    {
        [Inject]
        public IFhirService FhirService { get; set; }
        [Inject]
        public HttpClient Http { get; set; }
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public EventCallback<Questionnaire> SaveQuestionnaire { get; set; }
        public bool QLoaded { get; set; } = false;
        public Questionnaire Questionnaire { get; set; } = new Questionnaire();
        public QuestionnaireResponse QResponse { get; set; } = new QuestionnaireResponse();
        public IList<QuestionnaireResponse> QuestionnaireResponses { get; set; } = new List<QuestionnaireResponse>();
        public Dictionary<String, List<Coding>> AnswerOptionsDictionary { get; set; } = new Dictionary<string, List<Coding>>();
        public Dictionary<String, Questionnaire.QuestionnaireItemType> QuestionTypesDictionary { get; set; } = new Dictionary<string, Questionnaire.QuestionnaireItemType>();
        public string SubmittedMessage { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                Questionnaire = await FhirService.GetQuestionnaireByIdAsync(Id);
                QLoaded = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                // do error handling things here
            }
        }

        protected Questionnaire GenerateQR(Questionnaire Q)
        {
            Questionnaire q = new Questionnaire();
          
            foreach (var item in Questionnaire.Item)
            {
                q.Item.Add(GetItem(item));
            }
            return q;
        }
  
        protected Questionnaire.ItemComponent GetItem(Questionnaire.ItemComponent qItem)
        {
            var Item = new Questionnaire.ItemComponent();
            Item.Text = qItem.Text;
            Item.LinkId = qItem.LinkId;
            QuestionTypesDictionary.Add(qItem.LinkId, (Questionnaire.QuestionnaireItemType)qItem.Type);
            if(qItem.Type == Questionnaire.QuestionnaireItemType.Group)
            { 
                Item.Item = GetItems(qItem.Item);
            }
            
            return Item;
        }

        protected List<Questionnaire.ItemComponent> GetItems(List<Questionnaire.ItemComponent> qItems)
        {
            var Items = new List<Questionnaire.ItemComponent>();
            foreach (var item in qItems)
            {
                switch (item.Type)
                {

                    case Questionnaire.QuestionnaireItemType.Group:
                        Items = GetItems(item.Item);
                        break;
                    default:
                        Items.Add(GetItem(item));
                        break;
                }
            }

            return Items;
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
            return (item.Type.Equals(Questionnaire.QuestionnaireItemType.Group)) ? "Group" : $"{item.Type.ToString()} Question";
        }
        public async void Submit()
        {
            Questionnaire.Status = PublicationStatus.Draft;
            await SaveQuestionnaire.InvokeAsync(Questionnaire);
        }

    }
}
