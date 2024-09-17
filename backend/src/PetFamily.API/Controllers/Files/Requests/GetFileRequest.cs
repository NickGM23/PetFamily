using PetFamily.Application.Files.Get.GetFile;

namespace PetFamily.API.Controllers.Files.Requests
{
    public record GetFileRequest(string BucketName, string Path)
    {
        public GetFileCommand ToCommand() =>
            new(BucketName, Path);
    }
}
