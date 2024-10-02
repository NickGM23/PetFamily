
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.UpdatePet
{
    public record UpdatePetCommand(
        Guid VolunteerId,
        Guid PetId,
        string Name,
        string Description,
        string Color,
        string HealthInfo,
        Guid SpeciesId,
        Guid BreedId,
        AddressDto Address,
        int Weight,
        int Height,
        string PhoneNumber,
        bool IsCastrated,
        DateOnly BirthDate,
        bool IsVaccinated,
        string HelpStatus,
        IEnumerable<RequisiteDto> Requisites) : ICommand;
}
