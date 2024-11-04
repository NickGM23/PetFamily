using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain
{
    public class User : IdentityUser<Guid>
    {
        public string PhotoPath { get; set; } = string.Empty;

        public FullName FullName { get; set; } = default!;

        public SocialNetworkList SocialNetworks { get; set; } = null!;

        public Guid RoleId { get; set; }
        public Role Role { get; set; } = default!;

        public static User CreateAdmin(
            FullName fullName,
            SocialNetworkList socialNetworks,
            string userName, 
            string email, 
            Role role)
        {
            return new User
            {
                PhotoPath = "",
                FullName = fullName,
                SocialNetworks = socialNetworks,
                UserName = userName,
                Email = email,
                Role = role
            };
        }

        public static User CreateParticipant(
            FullName fullName,
            string userName, 
            string email, 
            Role role)
        {
            return new User
            {
                PhotoPath = "",
                SocialNetworks = new SocialNetworkList(new List<SocialNetwork>()),
                FullName = fullName,
                UserName = userName,
                Email = email,
                Role = role
            };
        }
    }
}
