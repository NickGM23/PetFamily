using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Domain.Models
{
    public class Species : Shared.Entity<SpeciesId>
    {
        public string Name { get; private set; } = string.Empty;

        public Description Description { get; private set; } 

        public IReadOnlyList<Breed> Breeds { get; private set; } = [];

        private Species(SpeciesId id) : base(id)
        {
        }

        private Species(SpeciesId id, string name, Description description)
            : base(id)
        {
            Name = name;
            Description = description;
        }

        public static Result<Species, Error> Create(
            SpeciesId speciesId,
            string name,
            Description description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Errors.General.ValueIsInvalid("Name"); ;
            }

            var species = new Species(
                speciesId,
                name,
                description);
            return species;
        }
    }
}
