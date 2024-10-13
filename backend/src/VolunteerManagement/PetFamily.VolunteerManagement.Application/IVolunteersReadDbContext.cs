using PetFamily.Core.Dtos;

namespace PetFamily.VolunteerManagement.Application
{
    public interface IVolunteersReadDbContext
    {
        public IQueryable<VolunteerDto> Volunteers { get; }
        public IQueryable<PetDto> Pets { get; }
    }
}
