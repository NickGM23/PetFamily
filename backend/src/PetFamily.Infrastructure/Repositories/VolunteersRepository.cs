
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.Infrastructure.DbContexts;
using PetFamily.SharedKernel;
using PetFamily.VolunteerManagement.Domain;
using PetFamily.VolunteerManagement.Application;

namespace PetFamily.Infrastructure.Repositories
{
    public class VolunteersRepository : IVolunteersRepository
    {
        private readonly WriteDbContext _context;

        public VolunteersRepository(WriteDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            await _context.Volunteers.AddAsync(volunteer, cancellationToken);

            return volunteer.Id;
        }

        public async Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var res = await _context.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == VolunteerId.Create(id), cancellationToken);

            if (res is null)
            {
                return Errors.General.NotFound(id);
            }

            return res;
        }

        public Guid Save(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            _context.Volunteers.Attach(volunteer);

            return volunteer.Id;
        }

        public Guid Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
        {
            _context.Volunteers.Remove(volunteer);

            return volunteer.Id.Value;
        }
    }
}
