using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.VolunteerManagement.Application.Commands.UploadFilesToPet
{
    public record UploadFilesToPetCommand(
        Guid VolunteerId, 
        Guid PetId, 
        string BucketName, 
        IEnumerable<UploadFileDto> Files) : ICommand;
}
