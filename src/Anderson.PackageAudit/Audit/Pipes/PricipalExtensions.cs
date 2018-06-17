using System.Security.Principal;

namespace Anderson.PackageAudit.Audit.Pipes
{
    public static class PricipalExtensions
    {
        public static Account ToAccount(this IPrincipal principal)
        {
            return default(Account);
        }
    }
}