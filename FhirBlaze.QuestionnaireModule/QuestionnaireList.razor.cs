using FhirBlaze.SharedComponents.Services;
using Microsoft.AspNetCore.Components;
using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using Microsoft.AspNetCore.Authorization;

namespace FhirBlaze.QuestionnaireModule
{
    [Authorize]
    public partial class QuestionnaireList
    {
        [Inject]
        public IFhirService FhirService { get; set; }   
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        protected bool ShowCreate { get; set; } = false;
        protected bool ShowSearch { get; set; } = false;
        protected bool Loading { get; set; } = true;
        protected bool ProcessingCreate { get; set; } = false;
        public IList<Questionnaire> Questionnaires { get; set; } = new List<Questionnaire>();

        private void CreateQuestionnaire(Questionnaire questionnaire)
        {

        }

        protected override async Task OnInitializedAsync()
        {
            Loading = true;
            Questionnaires = await FhirService.GetQuestionnairesAsync();
            Loading = false;
            ShouldRender();
        }

        public void ToggleCreate()
        {
            ShowCreate = !ShowCreate;
        }
        public void ToggleSearch()
        {
            ShowSearch = !ShowSearch;
        }

        public void OnRespondClick(string id)
        {
            NavigationManager.NavigateTo($"/questionnaire/{id}");
        }
    }
}
