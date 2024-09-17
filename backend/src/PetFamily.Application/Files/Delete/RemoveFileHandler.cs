
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Files.Delete
{
    public class RemoveFileHandler
    {
        private ILogger<RemoveFileHandler> _logger;
        private IFileProvider _fileProvider;

        public RemoveFileHandler(IFileProvider fileProvider, ILogger<RemoveFileHandler> logger)
        {
            _fileProvider = fileProvider;
            _logger = logger;
        }

        public async Task<Result<string, Error>> Handle(
            RemoveFileCommand command,
            CancellationToken cancellationToken = default)
        {
            var fileInfo = new FileProvider.FileInfo(command.BucketName, command.Path);

            var result = await _fileProvider.Remove(fileInfo, cancellationToken);

            _logger.LogInformation("File with path '{path}' has been deleted from bucket '{bucket}'",
                fileInfo.Path, fileInfo.BucketName);

            return result.Value;
        }
    }
}
