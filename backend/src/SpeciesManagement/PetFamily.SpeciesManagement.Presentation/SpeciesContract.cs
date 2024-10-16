
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Dtos;
using PetFamily.SharedKernel;
using PetFamily.SpeciesManagement.Application;
using PetFamily.SpeciesManagement.Contract;

namespace PetFamily.SpeciesManagement.Presentation
{
    public class SpeciesContract(ISpeciesReadDbContext readDbContext) : ISpeciesContract
    {
        public async Task<Result<SpeciesDto, Error>> GetSpeciesById(
            Guid speciesId, CancellationToken cancellationToken)
        {
            var result = await readDbContext.Species.Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => s.Id == speciesId, cancellationToken);

            if (result is not null)
                return result;

            return Errors.General.NotFound(speciesId);
        }
    }
}
