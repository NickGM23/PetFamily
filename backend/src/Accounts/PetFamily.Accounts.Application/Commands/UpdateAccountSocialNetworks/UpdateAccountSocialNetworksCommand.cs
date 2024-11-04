using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Accounts.Application.Commands.UpdateAccountSocialNetworks
{
    public record UpdateAccountSocialNetworksCommand(
        Guid UserId, 
        IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;
}
