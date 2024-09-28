
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Species;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.SpeciesManagement;

namespace PetFamily.Infrastructure.Repositories
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly ApplicationDbContext _context;

        public SpeciesRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<Guid> Add(Species species, CancellationToken cancellationToken = default)
        {
            await _context.Species.AddAsync(species, cancellationToken);

            return species.Id;
        }

        public Guid Delete(Species species, CancellationToken cancellationToken = default)
        {
            _context.Species.Remove(species);

            return species.Id.Value;
        }

        public async Task<Result<Species, Error>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var res = await _context.Species
                .FirstOrDefaultAsync(v => v.Id == SpeciesId.Create(id), cancellationToken);

            if (res is null)
            {
                return Errors.General.NotFound(id);
            }

            return res;
        }

        public Guid Save(Species species, CancellationToken cancellationToken = default)
        {
            _context.Species.Attach(species);

            return species.Id;
        }
    }
}
