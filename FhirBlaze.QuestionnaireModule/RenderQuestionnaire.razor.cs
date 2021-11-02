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
        public IList<QuestionnaireResponse> QuestionnaireResponses { get; set; } = new List<QuestionnaireResponse>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Questionnaire = await FhirService.GetQuestionnaireByIdAsync(Id);
                QuestionnaireResponses = await FhirService.GetQuestionnaireResponsesByQuestionnaireIdAsync(Id);               
            }
            catch (Exception)
            {
                // do error handling things here
            }                       

            ShouldRender();
        }
    }
}
