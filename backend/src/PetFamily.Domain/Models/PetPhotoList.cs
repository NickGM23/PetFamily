
namespace PetFamily.Domain.Models
{
    public class PetPhotoList
    {
        private List<PetPhoto> _petPhoto = [];

        public IReadOnlyList<PetPhoto> PetPhotos => _petPhoto.AsReadOnly();

        public void AddPetPhoto(PetPhoto petPhoto)
        {
            _petPhoto.Add(petPhoto);
        }
    }
}
