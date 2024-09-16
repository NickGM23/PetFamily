
using PetFamily.Application.Files.Upload;

namespace PetFamily.API.Controllers.Files.Requests
{
    public record UploadFileRequest(Stream Stream, string BucketName)
    {
        public UploadFileCommand ToCommand() =>
            new(Stream, BucketName);
    }
}
