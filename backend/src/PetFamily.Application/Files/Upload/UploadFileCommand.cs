
namespace PetFamily.Application.Files.Upload
{
    public record UploadFileCommand(Stream Stream, string BucketName);
}
