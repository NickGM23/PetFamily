
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species
{
    public interface ISpeciesRepository
    {
        Task<Guid> Add(PetFamily.Domain.SpeciesManagement.Species species, CancellationToken cancellationToken = default);

        Task<Result<PetFamily.Domain.SpeciesManagement.Species, Error>> GetById(Guid id, CancellationToken cancellationToken = default);

        Task<Result<PetFamily.Domain.SpeciesManagement.Species, Error>> GetByName(string speciesName, CancellationToken cancellationToken = default);

        Guid Save(PetFamily.Domain.SpeciesManagement.Species species, CancellationToken cancellationToken = default);

        Guid Delete(PetFamily.Domain.SpeciesManagement.Species species, CancellationToken cancellationToken = default);
    }
}
