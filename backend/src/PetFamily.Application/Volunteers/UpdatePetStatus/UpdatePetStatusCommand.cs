
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.UpdatePetStatus
{
    public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, string Status) : ICommand;
}
