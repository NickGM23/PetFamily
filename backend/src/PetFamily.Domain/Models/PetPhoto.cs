
namespace PetFamily.Domain.Models
{
    public record PetPhoto
    {
        public string Path { get; } = string.Empty;
        public bool IsMain { get; }

    }
}
