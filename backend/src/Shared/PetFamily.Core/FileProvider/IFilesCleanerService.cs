namespace PetFamily.Core.FileProvider
{
    public interface IFilesCleanerService
    {
        Task Process(CancellationToken cancellationToken);
    }
}
