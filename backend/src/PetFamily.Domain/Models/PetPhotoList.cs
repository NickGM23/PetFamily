
namespace PetFamily.Domain.Models
{
    public record PetPhotoList
    {

        public IReadOnlyList<PetPhoto> PetPhotos { get; private set; } = null!;

    }
}
