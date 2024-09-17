
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Get.GetFiles
{
    public class GetFilesHandler
    {
        private ILogger<GetFilesHandler> _logger;
        private IFileProvider _fileProvider;

        public GetFilesHandler(IFileProvider fileProvider, ILogger<GetFilesHandler> logger)
        {
            _fileProvider = fileProvider;
            _logger = logger;
        }

        public async Task<Result<List<string>, Error>> Handle(
            GetFilesCommand command,
            CancellationToken cancellationToken = default)
        {
            List<FileProvider.FileInfo> filesInfo = [];
            foreach (var getFileRequest in command.GetFileCommands)
            {
                var fileInfo = new FileProvider.FileInfo(getFileRequest.BucketName, getFileRequest.Path);

                filesInfo.Add(fileInfo);
            }

            var result = await _fileProvider.GetFiles(filesInfo, cancellationToken);
            if (result.IsFailure)
                return result.Error;

            return result;
        }
    }
}
