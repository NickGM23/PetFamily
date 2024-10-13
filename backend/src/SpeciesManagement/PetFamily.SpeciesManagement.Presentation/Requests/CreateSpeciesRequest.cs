using PetFamily.SpeciesManagement.Application.Commands.CreateSpecies;

namespace PetFamily.SpeciesManagement.Presentation.Requests
{
    public record CreateSpeciesRequest(
        string Name,
        string Description)
    {
        public CreateSpeciesCommand ToCommand() =>
            new(Name, Description);
    }
}
