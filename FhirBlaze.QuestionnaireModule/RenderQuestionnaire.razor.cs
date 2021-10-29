using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.QuestionnaireModule
{
    public partial class RenderQuestionnaire
    {
        [Parameter]
        public string Id { get; set; }
    }
}
