using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Domain.Models
{
    public class Species : Shared.Entity<SpeciesId>, ISoftDeletable
    {
        private bool _isDeleted = false;

        private readonly List<Breed> _breeds = [];

        public Name Name { get; private set; } = default!;

        public Description Description { get; private set; } = default!;

        public IReadOnlyList<Breed> Breeds => _breeds;

        private Species(SpeciesId id) : base(id)
        {
        }

        public Species(SpeciesId id, Name name, Description description)
            : base(id)
        {
            Name = name;
            Description = description;
        }

        public void Delete()
        {
            if (_isDeleted == false)
                _isDeleted = true;

            foreach (var breed in _breeds)
                breed.Delete();
        }

        public void Restore()
        {
            if (!_isDeleted) return;

            _isDeleted = false;
            foreach (var breed in _breeds)
                breed.Restore();
        }
    }
}
