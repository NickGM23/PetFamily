
using PetFamily.Application.Volunteers.RemoveFilesFromPet;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record RemoveFilesFromPetRequest(Guid PetId)
    {
        public RemoveFilesFromPetCommand ToCommand(Guid VolunteerId, string BucketName) =>
            new(VolunteerId, PetId, BucketName);
    }
}
