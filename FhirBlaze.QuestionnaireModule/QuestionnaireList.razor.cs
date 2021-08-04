using FhirBlaze.SharedComponents.Services;
using Microsoft.AspNetCore.Components;
using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.QuestionnaireModule
{
    public partial class QuestionnaireList
    {
        [Inject]
        public IFhirService FBService { get; set; }

        protected bool ShowCreate { get; set; } = false;
        protected bool Loading { get; set; } = true;
        public IList<Questionnaire> Questionnaires { get; set; } = new List<Questionnaire>();

        private void Create()
        {

        }

        protected override async Task OnInitializedAsync()
        {

            Loading = true;
            await base.OnInitializedAsync();
            Questionnaires = await FBService.GetQuestionnaireAsync();
            Loading = false;
            ShouldRender();
        }

        public void ToggleCreate()
        {
            ShowCreate = !ShowCreate;
        }
    }
}
