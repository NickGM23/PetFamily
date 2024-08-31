
namespace PetFamily.Domain.Models
{
    public record SocialNetworkList
    {

        public IReadOnlyList<SocialNetwork> SocialNetworks { get; private set; } = null!;

    }
}
