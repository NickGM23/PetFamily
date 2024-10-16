using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.RemovePhotosFromPet
{
    public record RemovePhotosFromPetCommand(Guid VolunteerId, Guid PetId, string BucketName) : ICommand;
}
