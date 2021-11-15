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
        public string PatientName { get; set; }
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
        public string SubmittedMessage { get; set; }
       
        public async Task<bool>  SubmitQuestionnaireAsync(EditContext ec)
        {
            IsSubmitted = true;
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
                pat.Reference = PatientId;
                pat.Type = "Patient";
                pat.Display = PatientName;
                QResponse.Status = QuestionnaireResponse.QuestionnaireResponseStatus.Completed;
                QResponse.Author = pat;
                var qr=await FhirService.SaveQuestionnaireResponseAsync(QResponse);
                Console.WriteLine($"Saved!  ID: {qr.Id}");
                SubmittedMessage = "Questionnaire Saved! You may close the dialog";
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
                QResponse = GenerateQR(Questionnaire);
                QRLoaded = true;
                RenderComponent(QResponse.Item);
                StateHasChanged();
                //QuestionnaireResponses = await FhirService.GetQuestionnaireResponsesByQuestionnaireIdAsync(Id);
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
            qr.Authored = DateTime.Now.ToString("yyyy-MM-dd");
            qr.Questionnaire = "Questionnaire/" + Q.Id;
            qr.Item =  new List<QuestionnaireResponse.ItemComponent>();
            foreach (var item in Questionnaire.Item)
            { 
                qr.Item.Add(GetItem(item));                
            }
            return qr;
        }

       
        protected QuestionnaireResponse.AnswerComponent PresetAnswer(Questionnaire.ItemComponent qItem)
        {
            var ans = new QuestionnaireResponse.AnswerComponent();
            switch (qItem.Type)
            {
                case Questionnaire.QuestionnaireItemType.String:
                    ans.Value = new FhirString("");
                    break;
                case Questionnaire.QuestionnaireItemType.Text:
                    ans.Value = new FhirString("");
                    break;
                case Questionnaire.QuestionnaireItemType.Decimal:
                    ans.Value = new FhirDecimal(1);
                    break;
                case Questionnaire.QuestionnaireItemType.Choice:
                    var c = new Coding();
                    var defaultCoding = (Coding)qItem.AnswerOption.First().Value;
                    List<Coding> ansOptions = new List<Coding>();
                    foreach (var option in qItem.AnswerOption)
                    {
                        ansOptions.Add((Coding)option.Value);
                    }
                    AnswerOptionsDictionary.Add(qItem.LinkId, ansOptions);
                    c.Code = defaultCoding.Code;
                    c.Display = defaultCoding.Display;
                    ans.Value = c;
                    break;
                default:
                    break;
            }
           return ans;
        }

        protected QuestionnaireResponse.ItemComponent GetItem(Questionnaire.ItemComponent qItem)
        {
            var Item = new QuestionnaireResponse.ItemComponent();
            Item.Text = qItem.Text;
            Item.LinkId = qItem.LinkId;
            QuestionTypesDictionary.Add(qItem.LinkId, (Questionnaire.QuestionnaireItemType)qItem.Type);
            var ansList = new List<QuestionnaireResponse.AnswerComponent>();
            switch (qItem.Type)
            {
                case Questionnaire.QuestionnaireItemType.Group:
                    
                    Item.Item = GetItems(qItem.Item);
                    break;
                default:
                    ansList.Add(PresetAnswer(qItem));
                    break;                 
            }
            if (ansList.Count > 0)
            {
                Item.Answer = ansList;
            }
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
