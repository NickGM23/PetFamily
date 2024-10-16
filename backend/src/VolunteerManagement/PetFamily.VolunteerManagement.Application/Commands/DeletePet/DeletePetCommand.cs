using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.DeletePet
{
    public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
}
