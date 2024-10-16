using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.VolunteerManagement.Application.Commands.CreateVolunteer
{
    public record CreateVolunteerCommand(
        FullNameDto FullName,
        string Email,
        string Description,
        int YearsExperience,
        string PhoneNumber,
        IEnumerable<SocialNetworkDto> SocialNetworks,
        IEnumerable<RequisiteDto> Requisites) : ICommand;
}
