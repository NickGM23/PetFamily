
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public record CreateVolunteerRequest(FullNameDTO FullName,
                                         string Email,
                                         string Description,
                                         int YearsOfExperience,
                                         string PhoneNumber,
                                         ICollection<SocialNetworkDTO> SocialNetworksDTO,
                                         ICollection<RequisiteDTO> RequisitesDTO);

    public record FullNameDTO(string LastName, 
                              string FirstName, 
                              string? Patronymic = null);

    public record SocialNetworkDTO(string Name, 
                                   string Link);

    public record RequisiteDTO(string Name, 
                               string Description);
}
