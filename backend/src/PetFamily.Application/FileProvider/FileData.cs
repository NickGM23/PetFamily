
namespace PetFamily.Application.FileProvider
{
    public record class FileData(Stream? Stream, string BucketName, string Path);
}
