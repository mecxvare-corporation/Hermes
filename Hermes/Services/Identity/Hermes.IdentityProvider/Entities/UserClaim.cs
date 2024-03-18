namespace Hermes.IdentityProvider.Entities
{
    public class UserClaim
    {
        public UserClaim()
        {
            Id = Guid.NewGuid().ToString();
        }

        public UserClaim(string claimType, string claimValue) : this()
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public UserClaim(string subjectId, string claimType, string claimValue) : this()
        {
            SubjectId = subjectId;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public string Id { get; set; }

        public string SubjectId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public virtual User User { get; set; }

    }
}
