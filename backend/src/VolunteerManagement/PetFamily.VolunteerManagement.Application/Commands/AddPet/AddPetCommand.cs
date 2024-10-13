using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.VolunteerManagement.Application.Commands.AddPet
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
        DateOnly BirthDate,
        bool IsVaccinated,
        string HelpStatus,
        IEnumerable<RequisiteDto> Requisites,
        Guid SpeciesId,
        Guid BreedId) : ICommand;
}
