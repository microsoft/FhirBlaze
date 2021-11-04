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
using System.Text.Json;
using System.Threading.Tasks;

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
        public string PatientId { get; set; }
        [Parameter]
        public bool IsPreview { get; set; } = false;
        public bool QRLoaded { get; set; } = false;
        public bool QLoaded { get; set; } = false;
        public Questionnaire Questionnaire { get; set; } = new Questionnaire();
        public QuestionnaireResponse QResponse { get; set; } = new QuestionnaireResponse();
        public IList<QuestionnaireResponse> QuestionnaireResponses { get; set; } = new List<QuestionnaireResponse>();
        public Dictionary<String, List<Coding>> AnswerOptionsDictionary { get; set; } = new Dictionary<string, List<Coding>>();
        public Dictionary<String, Questionnaire.QuestionnaireItemType> QuestionTypesDictionary { get; set; } = new Dictionary<string, Questionnaire.QuestionnaireItemType>();     
       
        public async Task<bool>  SubmitQuestionnaireAsync(EditContext ec)
        {
            bool submitted = false;
            foreach(var item in QResponse.Item)
            {
                var qType = QuestionTypesDictionary[item.LinkId];
                if (qType.Equals(Questionnaire.QuestionnaireItemType.Choice))
                {
                    var setAnswer=(Coding) item.Answer.First().Value;
                    //update the display to match with code
                    foreach (var a in AnswerOptionsDictionary[item.LinkId])
                    {
                        if (a.Code == setAnswer.Code)
                        {
                            setAnswer.Display = a.Display;
                            item.Answer[0].Value = setAnswer;
                        }
                    }
                }  
            }
            if (!string.IsNullOrEmpty(PatientId)){
                var pat = new ResourceReference();
                pat.Reference=PatientId;
                pat.Type = "Patient";
                pat.Display = "Name placeholder";
                QResponse.Status = QuestionnaireResponse.QuestionnaireResponseStatus.Completed;
                QResponse.Authored = null;
                QResponse.Author = pat;
                var qr=await FhirService.SaveQuestionnaireResponseAsync(QResponse);
                Console.WriteLine($"Saved!  ID: {qr.Id}");
                submitted = true;
                
            }
            return submitted;
        }
       
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Questionnaire = await FhirService.GetQuestionnaireByIdAsync(Id);
                QLoaded = true;
                StateHasChanged();
                if (!IsPreview)
                {
                    QResponse = GenerateQR(Questionnaire);
                    QRLoaded = true;
                    StateHasChanged();
                }
                QuestionnaireResponses = await FhirService.GetQuestionnaireResponsesByQuestionnaireIdAsync(Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                // do error handling things here
            }
            

        }

        protected QuestionnaireResponse GenerateQR(Questionnaire Q)
        {
            QuestionnaireResponse qr = new QuestionnaireResponse();
            qr.Status = QuestionnaireResponse.QuestionnaireResponseStatus.InProgress;
            qr.Authored = DateTime.Now.ToString();
            qr.Item = GetItems(Questionnaire.Item);
            return qr;
        }

        protected QuestionnaireResponse.ItemComponent GetItem(Questionnaire.ItemComponent qItem)
        {
            var Item = new QuestionnaireResponse.ItemComponent();
            Item.Text = qItem.Text;
            Item.LinkId = qItem.LinkId;
            var ansList = new List<QuestionnaireResponse.AnswerComponent>();
            var ans = new QuestionnaireResponse.AnswerComponent();
            QuestionTypesDictionary.Add(qItem.LinkId, (Questionnaire.QuestionnaireItemType)qItem.Type);
            switch (qItem.Type)
            {
                case Questionnaire.QuestionnaireItemType.String:
                    ans.Value = new FhirString("No answer");
                    ansList.Add(ans);
                    break;
                case Questionnaire.QuestionnaireItemType.Text:
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
                    List<Coding> ansOptions = new List<Coding>();
                    foreach (var option in  qItem.AnswerOption)
                    {
                       ansOptions.Add((Coding)option.Value);  
                    }
                    AnswerOptionsDictionary.Add(qItem.LinkId, ansOptions);
                   
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
