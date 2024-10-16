﻿using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SpeciesManagement.Domain.Entities;

namespace PetFamily.SpeciesManagement.Domain
{
    public class Species : SharedKernel.Entity<SpeciesId>, ISoftDeletable
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

        public UnitResult<Error> AddBreed(Breed breed)
        {
            if (_breeds.Exists(b => b.Name == breed.Name))
                return Errors.General.AlreadyExist(nameof(breed));

            _breeds.Add(breed);

            return Result.Success<Error>();
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

        public Result<Breed, Error> GetBreedById(BreedId breedId)
        {
            var breed = _breeds.FirstOrDefault(b => b.Id == breedId);

            if (breed is null)
                return Errors.General.NotFound(breedId);

            return breed;
        }
    }
}
