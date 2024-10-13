using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.VolunteerManagement.Domain;

namespace PetFamily.VolunteerManagement.Application
{
    public interface IVolunteersRepository
    {
        Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);

        Task<Result<Volunteer, Error>> GetById(Guid id, CancellationToken cancellationToken = default);

        Guid Save(Volunteer volunteer, CancellationToken cancellationToken = default);

        Guid Delete(Volunteer volunteer, CancellationToken cancellationToken = default);
    }
}
