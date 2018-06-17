using System.Security.Claims;
using System.Security.Principal;
using Anderson.PackageAudit.Domain;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public class WellKnownOpenIdConnectClaimTypes
    {
        public const string Issuer = "iss";
        public const string NameIdentifier = ClaimTypes.NameIdentifier;
    }
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