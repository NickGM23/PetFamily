
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.FileProvider
{
    public interface IFileProvider
    {
        Task<Result<string, Error>> Upload(FileData fileData, CancellationToken cancellationToken = default);
        Task<Result<string, Error>> Remove(FileData fileData, CancellationToken cancellationToken = default);
        Task<Result<string, Error>> GetFile(FileData fileData, CancellationToken cancellationToken = default);
        Task<Result<List<string>, Error>> GetFiles(
            IEnumerable<FileData> filesData,
            CancellationToken cancellationToken = default);
    }
}
