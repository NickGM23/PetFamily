using PetFamily.Core.Abstractions;

namespace PetFamily.SpeciesManagement.Application.Commands.CreateSpecies
{
    public record CreateSpeciesCommand(string Name, string Description) : ICommand;
}
