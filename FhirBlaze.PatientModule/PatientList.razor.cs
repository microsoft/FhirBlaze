using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.PatientModule
{
    public partial class PatientList
    {
        protected bool ShowCreate { get; set; } = false;

        
        public void ToggleCreate()
        {
            ShowCreate = !ShowCreate;
            ShouldRender();
        }
    }
}
