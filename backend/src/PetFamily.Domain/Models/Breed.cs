using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models
{
    public class Breed : Shared.Entity<BreedId>
    {
        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        private Breed(BreedId id) : base(id)
        {
        }

        private Breed(BreedId id,
                      string name,
                      string description) : base(id)
        {
            Name = name;
            Description = description;
        }

        public static Result<Breed> Create(BreedId breedId,
                                          string name,
                                          string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<Breed>("Name can not be empty");
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                return Result.Failure<Breed>("Description can not be empty");
            }

            var breed = new Breed(breedId,
                  name,
                  description);
            return breed;
        }
    }
}
