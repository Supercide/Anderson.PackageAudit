using System.Security.Claims;

namespace Anderson.PackageAudit.Infrastructure
{
    public class WellKnownOpenIdConnectClaimTypes
    {
        public const string Issuer = "iss";
        public const string NameIdentifier = ClaimTypes.NameIdentifier;
    }
}