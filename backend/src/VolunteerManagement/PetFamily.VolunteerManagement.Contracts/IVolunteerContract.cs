using CSharpFunctionalExtensions;
using PetFamily.Core.Dtos;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Contracts
{
    public interface IVolunteerContract
    {
        public Task<Result<PetDto, Error>> IsPetsUsedBreed(Guid breedId, CancellationToken cancellationToken);
        public Task<Result<PetDto, Error>> IsPetsUsedSpecies(Guid speciesId, CancellationToken cancellationToken);
    }
}
