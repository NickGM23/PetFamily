﻿using System.Reflection;

namespace PetFamily.SharedKernel.EntityIds
{
    public record VolunteerId
    {
        private VolunteerId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());

        public static VolunteerId Empty() => new(Guid.Empty);

        public static VolunteerId Create(Guid id) => new(id);

        public static implicit operator Guid(VolunteerId id)
        {
            ArgumentNullException.ThrowIfNull(id);
            return id.Value;
        }

        public static implicit operator VolunteerId(Guid id) => new(id);
    }
}