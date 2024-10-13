
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Core.FileProvider;
using PetFamily.SharedKernel;

namespace PetFamily.Application.Files.Get.GetFile
{
    public class GetFileHandler
    {
        private ILogger<GetFileHandler> _logger;
        private IFileProvider _fileProvider;

        public GetFileHandler(IFileProvider fileProvider, ILogger<GetFileHandler> logger)
        {
            _fileProvider = fileProvider;
            _logger = logger;
        }

        public async Task<Result<string, Error>> Handle(
            GetFileCommand comand,
            CancellationToken cancellationToken = default)
        {
            var fileInfo = new Core.FileProvider.FileInfo(comand.BucketName, comand.Path);

            var result = await _fileProvider.GetFile(fileInfo, cancellationToken);

            return result.Value;
        }
    }
}
