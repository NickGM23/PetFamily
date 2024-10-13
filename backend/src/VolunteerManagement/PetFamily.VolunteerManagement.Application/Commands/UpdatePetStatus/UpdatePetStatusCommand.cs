using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdatePetStatus
{
    public record UpdatePetStatusCommand(Guid VolunteerId, Guid PetId, string Status) : ICommand;
}
