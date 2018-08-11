using System.Security.Claims;
using System.Security.Principal;
using Anderson.PackageAudit.Domain;

namespace Anderson.PackageAudit.Infrastructure.Identity
{
    public static class PricipalExtensions
    {
        public static Account ToAccount(this IPrincipal principal)
        {
            var claimsPrincipal = (ClaimsPrincipal)principal;
            var issuer = claimsPrincipal.FindFirst(x => x.Type == WellKnownOpenIdConnectClaimTypes.Issuer);
            var authenticationId = claimsPrincipal.FindFirst(x => x.Type == WellKnownOpenIdConnectClaimTypes.NameIdentifier);
            return new Account(issuer.Value, authenticationId.Value);
        }
    }
}