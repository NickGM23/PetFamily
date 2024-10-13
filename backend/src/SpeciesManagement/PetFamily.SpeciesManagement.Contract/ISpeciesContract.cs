
using CSharpFunctionalExtensions;
using PetFamily.Core.Dtos;
using PetFamily.SharedKernel;

namespace PetFamily.SpeciesManagement.Contract
{
    public interface ISpeciesContract
    {
        public Task<Result<SpeciesDto, Error>> GetSpeciesById(
            Guid speciesId, CancellationToken cancellationToken);
    }
}
