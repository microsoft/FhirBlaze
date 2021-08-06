using Hl7.Fhir.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;

namespace FhirBlaze.PatientModule
{
    public partial class PatientSearch
    {
        [Parameter]
        public EventCallback<Patient> SearchPatient { get; set; } 

        [Parameter]
        public bool Processing { get; set; }

        [Parameter]
        public models.SimplePatient Patient { get; set; }

        [Parameter]
        public string familyName { get; set; }

        [Parameter]
        public string givenName{ get; set; }

        [Parameter]
        public string identifier { get; set; }



        protected async void searchPatient()
        {
           
            try
            {
               await SearchPatient.InvokeAsync(Patient.ToHl7Patient());
            }
            catch (Exception e)
            {

                var ttest = 3;
            }
            //var test = 1;
        }




        


        private AuthenticationResult GetAuthenticationResult()
        {
            string authority = "https://login.microsoftonline.com/72f988bf-86f1-41af-91ab-2d7cd011db47";
            string audience = "https://alangulotest.azurehealthcareapis.com/";
            string clientId = "3747a6e4-8b06-4dfc-84cf-668c723ae265";
            string clientSecret = "XYte6q-vpeX-4dnB6~Jg.Ris0Bw.9myHN2";

            AuthenticationContext authContext;
            ClientCredential clientCredential;

            authContext = new AuthenticationContext(authority);
            clientCredential = new ClientCredential(clientId, clientSecret);
            return authContext.AcquireTokenAsync(audience, clientCredential).Result;
        }




















    }




    public class OAuthResultModel
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
        [JsonProperty("experies_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("ext_experies_in")]
        public int ExtExpiresIn { get; set; }
        [JsonProperty("experies_on")]
        public int ExpiresOn { get; set; }
        [JsonProperty("not_before")]
        public int NotBefore { get; set; }
        [JsonProperty("resource")]
        public Uri Resource { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
