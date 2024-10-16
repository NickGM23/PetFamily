
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel;
using PetFamily.VolunteerManagement.Domain.ValueObjects;
using PetFamily.Core.FileProvider;
using PetFamily.Core.Extensions;
using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.UploadFilesToPet
{
    public class UploadFilesToPetHandler : ICommandHandler<Guid, UploadFilesToPetCommand>
    {
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _repository;
        private readonly IVolunteerUnitOfWork _unitOfWork;
        private readonly IValidator<UploadFilesToPetCommand> _validator;
        private readonly ILogger<UploadFilesToPetHandler> _logger;

        public UploadFilesToPetHandler(
            IFileProvider fileProvider,
            IVolunteersRepository volunteersRepository,
            IVolunteerUnitOfWork unitOfWork,
            IValidator<UploadFilesToPetCommand> validator,
            ILogger<UploadFilesToPetHandler> logger)
        {
            _fileProvider = fileProvider;
            _repository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UploadFilesToPetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToList();

            var volunteerResult = await _repository
                .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petId = PetId.Create(command.PetId);

            var petResult = volunteerResult.Value.GetPetById(petId);
            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            List<FileData> filesData = [];
            foreach (var file in command.Files)
            {
                var extension = Path.GetExtension(file.FileName);

                var filePath = FilePath.Create(Guid.NewGuid(), extension);
                if (filePath.IsFailure)
                    return filePath.Error.ToErrorList();

                var fileInfo = new Core.FileProvider.FileInfo(command.BucketName, filePath.Value.Path);

                var fileData = new FileData(file.Content, fileInfo);

                filesData.Add(fileData);
            }

            var filePathsResult = await _fileProvider.Uploads(filesData, cancellationToken);
            if (filePathsResult.IsFailure)
                return filePathsResult.Error.ToErrorList();

            var petPhotos = filePathsResult.Value
                .Select(f => PetPhoto.Create(FilePath.Create(f).Value, false).Value)
                .ToList();

            petResult.Value.UpdatePhotos(petPhotos);

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Success uploaded photos to pet - {petId}", petId.Value);

            return petResult.Value.Id.Value;
        }
    }
}
