using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Files.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Files.Delete;
using PetFamily.Application.Files.Get.GetFile;
using PetFamily.Application.Files.Get.GetFiles;
using PetFamily.Application.Files.Upload;

namespace PetFamily.API.Controllers.Files
{
    public class FilesController : ApplicationController
    {
        private const string BUCKET_NAME = "photos";

        [HttpPost]
        public async Task<IActionResult> UploadFile(
            IFormFile file,
            [FromServices] UploadFileHandler handler,
            CancellationToken cancellationToken)
        {
            await using var stream = file.OpenReadStream();

            var request = new UploadFileRequest(stream, BUCKET_NAME);

            var result = await handler.Handle(request.ToCommand(), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{path:guid}")]
        public async Task<IActionResult> RemoveFile(
            [FromRoute] Guid path,
            [FromServices] RemoveFileHandler handler,
            CancellationToken cancellationToken)
        {
            var request = new RemoveFileRequest(BUCKET_NAME, path.ToString());

            var result = await handler.Handle(request.ToCommand(), cancellationToken);

            return Ok(result.Value);
        }

        [HttpGet("{path:guid}")]
        public async Task<IActionResult> GetFile(
            [FromRoute] Guid path,
            [FromServices] GetFileHandler handler,
            CancellationToken cancellationToken)
        {
            var request = new GetFileRequest(BUCKET_NAME, path.ToString());

            var result = await handler.Handle(request.ToCommand(), cancellationToken);

            return Ok(result.Value);
        }

        [HttpPost("photos")]
        public async Task<IActionResult> GetFiles(
            [FromBody] IEnumerable<GetFileRequest> filesToGet,
            [FromServices] GetFilesHandler handler,
            CancellationToken cancellationToken)
        {
            var request = new GetFilesRequest(filesToGet);

            var result = await handler.Handle(request.ToCommand(), cancellationToken);

            return Ok(result.Value);
        }
    }
}
