using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDto> socialNetworks)
    {
        public UpdateSocialNetworksCommand ToCommand(Guid volunteerId) =>
            new(volunteerId, socialNetworks);
    }
}
