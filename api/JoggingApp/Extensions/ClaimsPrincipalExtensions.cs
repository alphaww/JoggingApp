using System;
using System.Security.Claims;

namespace JoggingApp
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetId(this ClaimsPrincipal principal)
        {
           var claimValue = principal.FindFirst("id")?.Value;
            var parsed = Guid.TryParse(claimValue, out Guid id);
            if (!parsed)
                throw new InvalidOperationException("Error parsing id claim from JWT. Please make sure there is id claim present.");
            return id;
        }
    }
}
