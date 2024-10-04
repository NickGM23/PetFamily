using PetFamily.Application.Volunteers.RemovePhotosFromPet;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record RemovePhotosFromPetRequest(Guid PetId)
    {
        public RemovePhotosFromPetCommand ToCommand(Guid VolunteerId, string BucketName) =>
            new(VolunteerId, PetId, BucketName);
    }
}
