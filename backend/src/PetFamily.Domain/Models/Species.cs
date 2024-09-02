using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models
{
    public class Species : Shared.Entity<SpeciesId>
    {
        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public IReadOnlyList<Breed> Breeds { get; private set; } = [];

        private Species(SpeciesId id) : base(id)
        {
        }

        private Species(SpeciesId id,
              string name,
              string description) : base(id)
        {
            Name = name;
            Description = description;
        }

        public static Result<Species> Create(SpeciesId speciesId,
                                          string name,
                                          string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<Species>("Name can not be empty");
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                return Result.Failure<Species>("Description can not be empty");
            }

            var species = new Species(speciesId,
                  name,
                  description);
            return species;
        }
    }
}
