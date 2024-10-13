using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.SpeciesManagement.Domain.Entities
{
    public class Breed : Entity<BreedId>, ISoftDeletable
    {
        private bool _isDeleted = false;

        public Name Name { get; private set; } = default!;

        public Description Description { get; private set; } = default!;

        private Breed(BreedId id) : base(id)
        {
        }

        public Breed(BreedId id, Name name, Description description)
            : base(id)
        {
            Name = name;
            Description = description;
        }

        public void Delete()
        {
            if (_isDeleted)
                return;

            _isDeleted = true;
        }

        public void Restore()
        {
            if (!_isDeleted)
                return;

            _isDeleted = false;
        }
    }
}
