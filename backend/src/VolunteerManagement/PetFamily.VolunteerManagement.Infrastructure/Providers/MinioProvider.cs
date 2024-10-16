﻿
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio.DataModel.Args;
using Minio;
using FileInfo = PetFamily.Core.FileProvider.FileInfo;
using IFileProvider = PetFamily.Core.FileProvider.IFileProvider;
using PetFamily.SharedKernel;
using PetFamily.Core.FileProvider;

namespace PetFamily.VolunteerManagement.Infrastructure.Providers
{
    public class MinioProvider : IFileProvider
    {
        private const int MAX_DEGREE_OF_PARALLELISM = 10;

        private IMinioClient _minioClient;
        private ILogger<MinioProvider> _logger;

        public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
        {
            _logger = logger;
            _minioClient = minioClient;
        }

        public async Task<Result<string, Error>> Upload(
            FileData fileData,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await BucketExistsCheck(fileData.FileInfo.BucketName, cancellationToken);

                var path = Guid.NewGuid();

                var fileArgs = new PutObjectArgs()
                    .WithObject(path.ToString())
                    .WithBucket(fileData.FileInfo.BucketName)
                    .WithObjectSize(fileData.Stream.Length)
                    .WithStreamData(fileData.Stream);

                var result = await _minioClient.PutObjectAsync(fileArgs, cancellationToken);

                return result.ObjectName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to upload file in minio");
                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
        }

        public async Task<Result<IReadOnlyList<string>, Error>> Uploads(
            IEnumerable<FileData> filesData,
            CancellationToken cancellationToken = default)
        {
            var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);
            var filesList = filesData.ToList();

            try
            {
                await IfBucketsNotExistCreateBuckets(filesList, cancellationToken);

                var tasks = filesList.Select(async file =>
                    await PutObject(file, semaphoreSlim, cancellationToken));

                var pathResult = await Task.WhenAll(tasks);

                if (pathResult.Any(p => p.IsFailure))
                    return pathResult.First().Error;

                var results = pathResult.Select(p => p.Value.Path).ToList();

                _logger.LogInformation("Uploaded files {files}", results);

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to upload files in minio, files amount: {amount}", filesList.Count);

                return Error.Failure("file.upload", "Fail to upload files in minio");
            }
        }

        public async Task<Result<string, Error>> Remove(
            FileInfo fileInfo,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await BucketExistsCheck(fileInfo.BucketName, cancellationToken);

                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(fileInfo.BucketName)
                    .WithObject(fileInfo.Path);

                await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

                return fileInfo.Path;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to delete file in minio");
                return Error.Failure("file.delete", "Fail to delete file in minio");
            }
        }

        public async Task<UnitResult<ErrorList>> DeleteFiles(
            IEnumerable<FileInfo> files,
            CancellationToken cancellationToken = default)
        {
            var semaphoreSlim = new SemaphoreSlim(MAX_DEGREE_OF_PARALLELISM);

            var filesList = files.ToList();

            try
            {
                var tasks = filesList.Select(async file =>
                    await DeleteFile(file, semaphoreSlim, cancellationToken));

                var deleteResults = await Task.WhenAll(tasks);

                if (deleteResults.Any(p => p.IsFailure))
                    return new ErrorList(deleteResults.Select(f => f.Error));

                _logger.LogInformation("deleted {count} files from minio", deleteResults.Length);

                return UnitResult.Success<ErrorList>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to upload files in minio, files amount: {amount}", filesList.Count);

                return Error.Failure("file.upload", "Fail to upload files in minio")
                    .ToErrorList();
            }
        }

        private async Task<UnitResult<Error>> DeleteFile(
            FileInfo fileInfo,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);

            var statArgs = new StatObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.Path);

            var statFile = await _minioClient.StatObjectAsync(statArgs);

            if (statFile.ContentType == null)
            {
                _logger.LogError(
                    "File with bucket '{bucket}', and name '{path}' not found",
                    fileInfo.BucketName,
                    fileInfo.Path);
                return Error.NotFound("file.not.found", $"file '{fileInfo.Path}' not found");
            }

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(fileInfo.BucketName)
                .WithObject(fileInfo.Path);

            try
            {
                await _minioClient
                    .RemoveObjectAsync(removeObjectArgs, cancellationToken);

                return UnitResult.Success<Error>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to delete file '{path}' in bucket '{bucket}' from minio",
                    fileInfo.Path,
                    fileInfo.BucketName);

                return Error.Failure("file.delete", "Fail to delete file from minio");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task<Result<string, Error>> GetFile(
            FileInfo fileInfo,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await BucketExistsCheck(fileInfo.BucketName, cancellationToken);

                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(fileInfo.BucketName)
                    .WithObject(fileInfo.Path)
                    .WithExpiry(60 * 60);

                return await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File on the path '{path}' does not exists", fileInfo.Path);
                return Error.Failure("file.get", $"The file can not be found at the path '{fileInfo.Path}'");
            }
        }

        public async Task<Result<List<string>, Error>> GetFiles(
            IEnumerable<FileInfo> filesInfo,
            CancellationToken cancellationToken = default)
        {
            List<string> filesLinks = [];
            foreach (var fileInfo in filesInfo)
            {
                try
                {
                    await BucketExistsCheck(fileInfo.BucketName, cancellationToken);

                    var presignedGetObjectArgs = new PresignedGetObjectArgs()
                        .WithBucket(fileInfo.BucketName)
                        .WithObject(fileInfo.Path)
                        .WithExpiry(60 * 60);

                    filesLinks.Add(await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "File on the path '{path}' does not exists", fileInfo.Path);
                    return Error.Failure("file.get", $"The file can not be found at the path '{fileInfo.Path}'");
                }
            }

            return filesLinks;
        }

        private async Task BucketExistsCheck(
            string bucketName,
            CancellationToken cancellationToken = default)
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExists = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);
            if (bucketExists == false)
            {
                await CreateBucket(bucketName, cancellationToken);
            }
        }

        private async Task CreateBucket(string bucketName, CancellationToken cancellationToken = default)
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(bucketName);

            await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }

        private async Task<Result<FileInfo, Error>> PutObject(
            FileData fileData,
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken = default)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(fileData.FileInfo.BucketName)
                .WithStreamData(fileData.Stream)
                .WithObjectSize(fileData.Stream.Length)
                .WithObject(fileData.FileInfo.Path);

            try
            {
                await _minioClient
                    .PutObjectAsync(putObjectArgs, cancellationToken);

                return fileData.FileInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fail to upload file in minio with path '{path}' in bucket '{bucket}'",
                    fileData.FileInfo.Path,
                    fileData.FileInfo.BucketName);

                return Error.Failure("file.upload", "Fail to upload file in minio");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private async Task IfBucketsNotExistCreateBuckets(
            IEnumerable<FileData> filesData,
            CancellationToken cancellationToken = default)
        {
            HashSet<string> bucketNames = [.. filesData.Select(file => file.FileInfo.BucketName)];

            foreach (var bucketName in bucketNames)
            {
                var bucketExistArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);

                var bucketExist = await _minioClient
                    .BucketExistsAsync(bucketExistArgs, cancellationToken);

                if (bucketExist == false)
                {
                    await CreateBucket(bucketName, cancellationToken);
                }
            }
        }
    }
}