
namespace PetFamily.Application.Dtos
{
    public record UpdateVolunteerMainInfoDto(
        FullNameDto FullNameDto,
        string Email,
        string Description,
        int YearsExperience,
        string PhoneNumber);
}
