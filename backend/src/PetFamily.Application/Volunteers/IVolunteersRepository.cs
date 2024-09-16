using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteersManagement;

namespace PetFamily.Application.Volunteers
{
    public interface IVolunteersRepository
    {
        Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
        
        Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default);

        Task Save(Volunteer volunteer, CancellationToken cancellationToken = default);
    }
}
