
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.RemoveFilesFromPet
{
    public record RemoveFilesFromPetCommand(Guid VolunteerId, Guid PetId, string BucketName) : ICommand;
}
