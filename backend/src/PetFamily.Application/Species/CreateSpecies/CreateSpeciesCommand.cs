
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.CreateSpecies
{
    public record CreateSpeciesCommand(string Name, string Description) : ICommand;
}
