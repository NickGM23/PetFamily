
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.DeletePet
{
    public record DeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;
}
