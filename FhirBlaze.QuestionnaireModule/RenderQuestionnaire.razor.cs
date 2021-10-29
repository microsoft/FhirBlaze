using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.QuestionnaireModule
{
    public partial class RenderQuestionnaire
    {
        [Inject]
        public IFhirService FhirService { get; set; } 
        [Parameter]
        public string Id { get; set; }
        public Questionnaire Questionnaire { get; set; } = new Questionnaire();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Questionnaire = await FhirService.GetQuestionnaireByIdAsync(Id);
            }
            catch (Exception)
            {
                // do error handling things here
            }
            ShouldRender();
        }
    }
}
