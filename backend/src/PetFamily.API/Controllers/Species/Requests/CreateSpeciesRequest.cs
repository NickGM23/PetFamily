
using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Application.Species.CreateSpecies;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record CreateSpeciesRequest(
        string Name,
        string Description)
    {
        public CreateSpeciesCommand ToCommand() =>
            new(Name, Description);
     }
}
