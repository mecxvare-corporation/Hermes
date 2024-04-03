using Duende.IdentityServer.EntityFramework.Entities;
using System.ComponentModel.DataAnnotations;

namespace Hermes.IdentityProvider.Entities
{
    public class User
    {
        public User()
        {
            SubjectId = Guid.NewGuid().ToString();
            UserClaims = new List<UserClaim>();
            CreateDate = DateTime.UtcNow;
        }

        [Key]
        public string SubjectId { get; private set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreateDate { get; private set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<UserClaim> UserClaims { get; private set; }

        public User AddUserClaims(params UserClaim[] userClaims)
        {
            foreach (var userClaim in userClaims)
            {
                UserClaims.Add(userClaim);
            }
            return this;
        }
    }
}
