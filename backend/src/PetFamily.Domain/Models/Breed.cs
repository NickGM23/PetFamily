using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Domain.Models
{
    public class Breed : Shared.Entity<BreedId>
    {
        public string Name { get; private set; } = string.Empty;

        public Description Description { get; private set; } = default!;

        private Breed(BreedId id) : base(id)
        {
        }

        private Breed(BreedId id, string name, Description description)
            : base(id)
        {
            Name = name;
            Description = description;
        }

        public static Result<Breed, Error> Create(
            BreedId breedId,
            string name,
            Description description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Errors.General.ValueIsInvalid("Name");
            }

            var breed = new Breed(breedId,
                name,
                description);
            return breed;
        }
    }
}
