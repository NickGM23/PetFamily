using PetFamily.Core.Dtos;

namespace PetFamily.SpeciesManagement.Application
{
    public interface ISpeciesReadDbContext
    {
        public IQueryable<SpeciesDto> Species { get; }
        public IQueryable<BreedDto> Breeds { get; }
    }
}
