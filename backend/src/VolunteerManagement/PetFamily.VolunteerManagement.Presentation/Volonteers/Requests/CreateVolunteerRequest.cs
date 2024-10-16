using PetFamily.Core.Dtos;
using PetFamily.VolunteerManagement.Application.Commands.CreateVolunteer;

namespace PetFamily.VolunteerManagement.Presentation.Volonteers.Requests
{
    public record CreateVolunteerRequest(
        FullNameDto FullName,
        string Email,
        string Description,
        int YearsExperience,
        string PhoneNumber,
        IEnumerable<SocialNetworkDto> SocialNetworks,
        IEnumerable<RequisiteDto> Requisites)
    {
        public CreateVolunteerCommand ToCommand() =>
            new(FullName, Email, Description, YearsExperience, PhoneNumber, SocialNetworks, Requisites);
    }
}
