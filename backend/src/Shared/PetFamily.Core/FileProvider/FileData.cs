namespace PetFamily.Core.FileProvider
{
    public record FileData(Stream Stream, FileInfo FileInfo);
    public record FileInfo(string BucketName, string Path);

}
