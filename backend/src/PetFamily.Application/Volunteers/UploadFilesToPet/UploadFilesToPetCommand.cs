
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.UploadFilesToPet
{
    public record UploadFilesToPetCommand(Guid VolunteerId, Guid PetId, string BucketName, IEnumerable<UploadFileDto> Files);
}
