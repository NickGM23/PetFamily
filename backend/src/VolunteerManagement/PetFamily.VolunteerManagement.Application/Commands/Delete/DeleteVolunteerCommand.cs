using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.Delete
{
    public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;
}
