using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared
{
    public record FullName
    {
        public string LastName { get; }

        public string FirstName { get; }

        public string? Patronymic { get; }
        
        private FullName(string lastName, string firstName, string? patronymic)
        {
            FirstName = firstName;
            Patronymic = patronymic;
            LastName = lastName;
        }

        public static Result<FullName, Error> Create(string lastName, string firstName, string? patronymic = null)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return Errors.General.ValueIsInvalid("LastName");
            }
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return Errors.General.ValueIsInvalid("FirstName");
            }

            return new FullName(lastName, firstName, patronymic);
        }
    }
}
