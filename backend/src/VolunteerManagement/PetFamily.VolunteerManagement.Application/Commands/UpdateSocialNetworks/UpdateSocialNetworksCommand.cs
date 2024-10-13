using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateSocialNetworks
{
    public record UpdateSocialNetworksCommand(
        Guid VolunteerId, 
        IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;
}
