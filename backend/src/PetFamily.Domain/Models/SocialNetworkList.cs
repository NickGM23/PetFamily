﻿
namespace PetFamily.Domain.Models
{
    public record SocialNetworkList
    {
        public SocialNetworkList()
        {
        }

        public IReadOnlyList<SocialNetwork> SocialNetworks { get; }

        public SocialNetworkList(IEnumerable<SocialNetwork> socialNetworks)
        {
            SocialNetworks = socialNetworks.ToList();
        }
    }
}
