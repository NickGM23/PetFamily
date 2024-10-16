using PetFamily.VolunteerManagement.Application.Commands.RemovePhotosFromPet;

namespace PetFamily.VolunteerManagement.Presentation.Volonteers.Requests
{
    public record RemovePhotosFromPetRequest(Guid PetId)
    {
        public RemovePhotosFromPetCommand ToCommand(Guid VolunteerId, string BucketName) =>
            new(VolunteerId, PetId, BucketName);
    }
}
