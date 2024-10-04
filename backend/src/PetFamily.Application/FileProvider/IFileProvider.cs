
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileProvider
{
    public interface IFileProvider
    {
        Task<Result<string, Error>> Upload(FileData fileData, CancellationToken cancellationToken = default);

        Task<Result<IReadOnlyList<string>, Error>> Uploads(IEnumerable<FileData> filesData,
            CancellationToken cancellationToken = default);

        Task<Result<string, Error>> Remove(FileInfo fileInfo, CancellationToken cancellationToken = default);

        Task<UnitResult<ErrorList>> DeleteFiles(IEnumerable<FileInfo> files, CancellationToken cancellationToken = default);

        Task<Result<string, Error>> GetFile(FileInfo fileInfo, CancellationToken cancellationToken = default);

        Task<Result<List<string>, Error>> GetFiles(IEnumerable<FileInfo> filesInfo,
            CancellationToken cancellationToken = default);
    }
}
