namespace PetFamily.Domain.Shared
{
    public record FullName
    {
        public string FirstName { get; } = string.Empty;
        public string SecondName { get; } = string.Empty;
        public string LastName { get; } = string.Empty;
    }
}
