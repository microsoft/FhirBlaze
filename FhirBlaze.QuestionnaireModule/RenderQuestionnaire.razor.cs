using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using Task = System.Threading.Tasks.Task;
using Microsoft.AspNetCore.Components.Forms;
namespace FhirBlaze.QuestionnaireModule
{
    public partial class RenderQuestionnaire
    {
        [Inject]
        public IFhirService FhirService { get; set; } 
        [Inject]
        public HttpClient Http { get; set; }
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public bool IsPreview { get; set; } = false;
        public Questionnaire Questionnaire { get; set; } = new Questionnaire();
        public QuestionnaireResponse QResponse { get; set; } = new QuestionnaireResponse();
        public IList<QuestionnaireResponse> QuestionnaireResponses { get; set; } = new List<QuestionnaireResponse>();

        /*
         * Currently we render a "Questionnaire" object
         * Instead we shouldd create a QuestionnaireResponse Object and 
         * Render it as a EditForm
         * 
         * Next step: For a given Q create a QR
         * Next Step : Render the QR as an editform
         * Next step persist that new QR
         * https://www.claudiobernasconi.ch/2021/04/29/introduction-to-blazor-form-handling-and-input-validation/
         */
        public void SubmitQuestionnaire(EditContext ec)
        {
            Console.WriteLine("submit");
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Questionnaire = await FhirService.GetQuestionnaireByIdAsync(Id);
                QResponse = GenerateQR(Questionnaire);
                QuestionnaireResponses = await FhirService.GetQuestionnaireResponsesByQuestionnaireIdAsync(Id);               
            }
            catch (Exception)
            {
                // do error handling things here
            }                       

            ShouldRender();
        }

        protected QuestionnaireResponse GenerateQR(Questionnaire Q)
        {
            QuestionnaireResponse qr = new QuestionnaireResponse();
            qr.Status = QuestionnaireResponse.QuestionnaireResponseStatus.InProgress;
            qr.Authored = DateTime.Now.ToString();
            qr.Item = GetItems(Questionnaire.Item);
            /*
             * Debug code below....
             */
            foreach (var item in qr.Item)
            {
                foreach (var ans in item.Answer)
                {
                    var s=ans.Value.ToString();
                    var t = ans.Value.GetType().Name;
                    var text = item.Text;
                    Console.WriteLine("breakpoint");
                }
            }
            return qr;
        }
        protected QuestionnaireResponse.ItemComponent GetItem(Questionnaire.ItemComponent qItem)
        {
            var Item = new QuestionnaireResponse.ItemComponent();
            Item.Text = qItem.Text;
            Item.LinkId = qItem.LinkId;
            var ansList = new List<QuestionnaireResponse.AnswerComponent>();
            var ans = new QuestionnaireResponse.AnswerComponent();
            switch (qItem.Type)
            {
                case Questionnaire.QuestionnaireItemType.String:
                    ans.Value = new FhirString("No answer");
                    ansList.Add(ans);
                    break;
                case Questionnaire.QuestionnaireItemType.Text:
                    //should be string []
                    ans.Value = new FhirString("No answer text");
                    ansList.Add(ans);
                    break;
                case Questionnaire.QuestionnaireItemType.Decimal:
                    ans.Value = new FhirDecimal(1);
                    ansList.Add(ans);
                    break;
                case Questionnaire.QuestionnaireItemType.Choice:
                    var c = new Coding();
                    var defaultCoding =(Coding) qItem.AnswerOption.First().Value;
                    c.Code = defaultCoding.Code;
                    c.Display = defaultCoding.Display;
                    ans.Value = c;
                    ansList.Add(ans);
                    break;
                default:
                    break;

                 
            }
            
            Item.Answer = ansList;
            return Item;
        }
        protected List<QuestionnaireResponse.ItemComponent> GetItems(List<Questionnaire.ItemComponent> qItems)
        {
            var Items = new List<QuestionnaireResponse.ItemComponent>();
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

        
    }
}
