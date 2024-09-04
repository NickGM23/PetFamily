using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared
{
    public record Requisite
    {
        public string Name { get; } = string.Empty;

        public string Description { get; } = string.Empty;

        private Requisite(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public static Result<Requisite, Error> Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Errors.General.ValueIsInvalid("Name");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Errors.General.ValueIsInvalid("Description");
            }

            return new Requisite(name, description);
        }
    }
}
