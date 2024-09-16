using PetFamily.Application.Files.Delete;

namespace PetFamily.API.Controllers.Files.Requests
{
    public record RemoveFileRequest(string BucketName, string Path) 
    {
        public RemoveFileCommand ToCommand() =>
            new(BucketName, Path);
    }
}
