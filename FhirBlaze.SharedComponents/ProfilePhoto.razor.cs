using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FhirBlaze.SharedComponents
{
    [Authorize]
    public partial class ProfilePhoto
    {
        [Inject]
        GraphServiceClient graphClient { get; set; }

        public string ImageSource { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var photo = await graphClient.Me.Photos["48x48"].Content.Request().GetResponseAsync();
            if (photo.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var bytes = await photo.Content.ReadAsByteArrayAsync();

                ImageSource = $"data:image/png;base64,{Convert.ToBase64String(bytes)}";
            }
            else
            {
                ImageSource = "";
            }
        }


    }
}
