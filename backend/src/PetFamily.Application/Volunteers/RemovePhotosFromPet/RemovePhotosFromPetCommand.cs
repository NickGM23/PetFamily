using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.RemovePhotosFromPet
{
    public record RemovePhotosFromPetCommand(Guid VolunteerId, Guid PetId, string BucketName) : ICommand;
}
