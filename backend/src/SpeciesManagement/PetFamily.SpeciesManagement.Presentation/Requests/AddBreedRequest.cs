using PetFamily.SpeciesManagement.Application.Commands.AddBreed;

namespace PetFamily.SpeciesManagement.Presentation.Requests
{
    public record AddBreedRequest(
        string Name,
        string Description)
    {
        public AddBreedCommand ToCommand(Guid speciesId) =>
                new(speciesId, Name, Description);
    }
}
