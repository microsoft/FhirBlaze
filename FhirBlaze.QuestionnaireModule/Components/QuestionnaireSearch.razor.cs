using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.QuestionnaireModule.Components
{
    public partial class QuestionnaireSearch
    {
        [Parameter]
        public EventCallback<string> SearchClicked { get; set; }
        protected string SearchQuery { get; set; }
      
    }
}
