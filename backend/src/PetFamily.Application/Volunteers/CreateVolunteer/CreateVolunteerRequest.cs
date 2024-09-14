
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public record CreateVolunteerRequest(FullNameDto FullName,
                                         string Email,
                                         string Description,
                                         int YearsExperience,
                                         string PhoneNumber,
                                         ICollection<SocialNetworkDto> SocialNetworksDto,
                                         ICollection<RequisiteDto> RequisitesDto);
}
