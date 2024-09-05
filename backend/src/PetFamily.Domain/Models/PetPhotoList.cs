
namespace PetFamily.Domain.Models
{
    public record PetPhotoList
    {

        public IReadOnlyList<PetPhoto> PetPhotos { get; }

        public PetPhotoList(IEnumerable<PetPhoto> petPhotso)
        {
            PetPhotos = petPhotso.ToList();
        }

    }
}
