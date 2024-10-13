using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SpeciesManagement.Domain;

namespace PetFamily.SpeciesManagement.Application
{
    public interface ISpeciesRepository
    {
        Task<Guid> Add(Species species, CancellationToken cancellationToken = default);

        Task<Result<Species, Error>> GetById(Guid id, CancellationToken cancellationToken = default);

        Task<Result<Species, Error>> GetByName(string speciesName, CancellationToken cancellationToken = default);

        Guid Save(Species species, CancellationToken cancellationToken = default);

        Guid Delete(Species species, CancellationToken cancellationToken = default);
    }
}
