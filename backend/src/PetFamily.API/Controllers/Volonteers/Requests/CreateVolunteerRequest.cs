using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers.Volonteers.Requests
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
