
namespace PetFamily.Domain.Models.Shared
{
    public record FullName
    {
        public string FirstName { get; init; } = string.Empty;
        public string SecondName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
    }
}
