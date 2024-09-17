
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.AddPet
{
    public record AddPetCommand(
        Guid VolunteerId,
        string Name,
        string Description,
        string Color,
        string HealthInfo,
        AddressDto Address,
        double Weight,
        double Height,
        string PhoneNumber,
        bool IsCastrated,
        DateTime BirthDate,
        bool IsVaccinated,
        string HelpStatus,
        IEnumerable<RequisiteDto> Requisites);
}
