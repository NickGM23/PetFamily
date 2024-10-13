namespace PetFamily.VolunteerManagement.Domain.ValueObjects
{
    public record PetPhotoList
    {
        private PetPhotoList()
        {

        }

        public IReadOnlyList<PetPhoto> PetPhotos { get; }

        public PetPhotoList(IEnumerable<PetPhoto> petPhotos)
        {
            PetPhotos = petPhotos.ToList();
        }

    }
}
