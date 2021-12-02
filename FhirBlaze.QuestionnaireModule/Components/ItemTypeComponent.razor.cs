using FhirBlaze.SharedComponents.Services;
using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hl7.Fhir.Model.Questionnaire;
using Task = System.Threading.Tasks.Task;

namespace FhirBlaze.QuestionnaireModule.Components
{
    public partial class ItemTypeComponent
    {

        protected QuestionnaireItemType QuestionnaireItemType { get; set; }

        [Parameter]
        public EventCallback<QuestionnaireItemType> ItemSelected { get; set; }

        protected async Task SetItemType(QuestionnaireItemType selectedItem)
        {
            QuestionnaireItemType = selectedItem;
            await ItemSelected.InvokeAsync(QuestionnaireItemType);
            


        }
    }
}
