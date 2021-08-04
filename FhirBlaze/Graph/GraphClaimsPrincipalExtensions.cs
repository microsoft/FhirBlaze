using Microsoft.Graph;
using System;
using System.IO;
using System.Security.Claims;

namespace FhirBlaze.Graph
{
    public static class GraphClaimTypes
    {
        public const string Email = "graph_email";
        public const string Photo = "graph_photo";
    }

    // Helper methods to access Graph user data stored in
    // the claims principal
    public static class GraphClaimsPrincipalExtensions
    {
        public static string GetUserGraphEmail(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindFirst(GraphClaimTypes.Email);
            return claim == null ? null : claim.Value;
        }

        public static string GetUserGraphPhoto(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindFirst(GraphClaimTypes.Photo);
            return claim == null ? null : claim.Value;
        }

        // Adds claims from the provided User object
        public static void AddUserGraphInfo(this ClaimsPrincipal claimsPrincipal, User user)
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;

            identity.AddClaim(
                new Claim(GraphClaimTypes.Email,
                    user.Mail ?? user.UserPrincipalName));
        }

        // Converts a photo Stream to a Data URI and stores it in a claim
        public static void AddUserGraphPhoto(this ClaimsPrincipal claimsPrincipal, Stream photoStream)
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;

            if (photoStream != null)
            {
                // Copy the photo stream to a memory stream
                // to get the bytes out of it
                var memoryStream = new MemoryStream();
                photoStream.CopyTo(memoryStream);
                var photoBytes = memoryStream.ToArray();

                // Generate a date URI for the photo
                var photoUri = $"data:image/png;base64,{Convert.ToBase64String(photoBytes)}";

                identity.AddClaim(
                    new Claim(GraphClaimTypes.Photo, photoUri));
            }
        }
    }
}
