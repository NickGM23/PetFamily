using PetFamily.Core.Abstractions;

namespace PetFamily.SpeciesManagement.Application.Commands.Delete
{
    public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;
}
