﻿namespace PetFamily.Domain.Shared.ValueObjects.Ids
{
    public record PetId
    {
        private PetId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static PetId NewPetId() => new(Guid.NewGuid());

        public static PetId Empty() => new(Guid.Empty);

        public static PetId Create(Guid id) => new(id);

        public static implicit operator Guid(PetId id) => id.Value;
    }
}