using PetFamily.Core.Dtos;
using PetFamily.VolunteerManagement.Application.Commands.UpdateSocialNetworks;

namespace PetFamily.VolunteerManagement.Presentation.Volonteers.Requests
{
    public record UpdateSocialNetworksRequest(IEnumerable<SocialNetworkDto> socialNetworks)
    {
        public UpdateSocialNetworksCommand ToCommand(Guid volunteerId) =>
            new(volunteerId, socialNetworks);
    }
}
