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
        public NavigationManager NavigationManager { get; set; }
        [Parameter]
        public string Id { get; set; }
        public bool QLoaded { get; set; } = false;
        public Questionnaire Questionnaire { get; set; } = new Questionnaire();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                QLoaded = false;
                Questionnaire = await FhirService.GetQuestionnaireByIdAsync(Id);
                QLoaded = true;
                StateHasChanged();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }       

        protected string GetHeader(Questionnaire.ItemComponent item)
        {
            return (item.Type.Equals(Questionnaire.QuestionnaireItemType.Group)) ? "Group" : $"{item.Type.ToString()} Question";
        }
        public async Task Submit()
        {
            if (!string.IsNullOrEmpty(Id))
            {
                Questionnaire.Status = PublicationStatus.Draft;
                Questionnaire.Date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
                var questionnaire = await FhirService.UpdateQuestionnaireAsync(Questionnaire);
                NavigationManager.NavigateTo("/questionnaire");
                Console.WriteLine($"Saved!  ID: {questionnaire.Id}");
            }
        }
    }
}
