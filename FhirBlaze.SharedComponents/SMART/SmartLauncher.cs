using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FhirBlaze.SharedComponents.SMART
{
    public class SmartLauncher
    {
        private readonly string _launchUrl;
        private readonly string _authority;

        public SmartLauncher(string launchUrl, string authority)
        {
            _launchUrl = launchUrl;
            _authority = authority;
        }

        public string GetLaunchUrl(string payloadJson)
        {
            var result = new StringBuilder();
            result.Append(_launchUrl);
            // encode the json string
            var jsonEncoded = EncodePayload(payloadJson);
            result.Append($"?launch={jsonEncoded}");
            result.Append($"&iss={_authority}");

            return result.ToString();
        }

        public string EncodePayload(string payload)
        {
            var bytes = Encoding.UTF8.GetBytes(payload);
            return HttpUtility.UrlEncode(Convert.ToBase64String(bytes));
        }
    }
}
