
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.AddBreed
{
    public record AddBreedCommand(
        Guid SpeciesId,
        string Name,
        string Description) : ICommand;
}
