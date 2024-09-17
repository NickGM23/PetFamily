using PetFamily.Application.Files.Get.GetFiles;

namespace PetFamily.API.Controllers.Files.Requests
{
    public record GetFilesRequest(IEnumerable<GetFileRequest> GetFilesRequests)
    {
        public GetFilesCommand ToCommand() =>
            new(GetFilesRequests.Select(x => x.ToCommand()));
    }
}
