using System;

namespace Anderson.PackageAudit.Domain
{
    public class Account : IEquatable<Account>
    {
        public Account(string provider, string authenticationId)
        {
            Provider = provider;
            AuthenticationId = authenticationId;
        }

        public string Provider { get; protected set; }
        public string AuthenticationId { get; protected set; }

        public bool Equals(Account other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Provider, other.Provider) && string.Equals(AuthenticationId, other.AuthenticationId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Account) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Provider != null ? Provider.GetHashCode() : 0) * 397) ^ (AuthenticationId != null ? AuthenticationId.GetHashCode() : 0);
            }
        }
    }
}