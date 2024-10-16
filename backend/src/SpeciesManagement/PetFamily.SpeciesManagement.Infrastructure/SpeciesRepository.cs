﻿
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel;
using PetFamily.SpeciesManagement.Domain;
using PetFamily.SpeciesManagement.Application;

namespace PetFamily.SpeciesManagement.Infrastructure
{
    public class SpeciesRepository : ISpeciesRepository
    {
        private readonly SpeciesWriteDbContext _context;

        public SpeciesRepository(SpeciesWriteDbContext context)
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
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(v => v.Id == SpeciesId.Create(id), cancellationToken);

            if (res is null)
            {
                return Errors.General.NotFound(id);
            }

            return res;
        }

        public async Task<Result<Species, Error>> GetByName(string speciesName, CancellationToken cancellationToken = default)
        {
            var res = await _context.Species
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(v => v.Name.Value.ToLower().Equals(speciesName.ToLower()), cancellationToken);

            if (res is null)
            {
                return Errors.General.NotFound();
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
