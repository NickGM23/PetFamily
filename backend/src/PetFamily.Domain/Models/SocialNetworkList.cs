
namespace PetFamily.Domain.Models
{
    public class SocialNetworkList
    {
        private readonly List<SocialNetwork> _socialNetworks = [];

        public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

        public void AddSocialNetwork(SocialNetwork socialNetwork)
        {
            _socialNetworks.Add(socialNetwork);
        }
    }
}
