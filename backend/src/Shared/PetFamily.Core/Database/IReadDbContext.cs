using PetFamily.Core.Dtos;

namespace PetFamily.Core.Database
{
    public interface IReadDbContext
    {
        public IQueryable<VolunteerDto> Volunteers { get; }

        public IQueryable<SpeciesDto> Species { get; }

        public IQueryable<PetDto> Pets { get; }

        public IQueryable<BreedDto> Breeds { get; }
    }
}
