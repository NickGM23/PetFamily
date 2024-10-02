
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.Shared;
using FluentValidation;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.RemoveFilesFromPet
{

    public class RemoveFilesFromPetHandler : ICommandHandler<Guid, RemoveFilesFromPetCommand>
    {
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RemoveFilesFromPetCommand> _validator;
        private readonly ILogger<RemoveFilesFromPetHandler> _logger;

        public RemoveFilesFromPetHandler(
            IFileProvider fileProvider,
            IVolunteersRepository repository,
            IUnitOfWork unitOfWork,
            IValidator<RemoveFilesFromPetCommand> validator,
            ILogger<RemoveFilesFromPetHandler> logger)
        {
            _fileProvider = fileProvider;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(RemoveFilesFromPetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
                return validationResult.ToList();

            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteer = await _repository.GetById(volunteerId, cancellationToken);
            if (volunteer.IsFailure)
                return volunteer.Error.ToErrorList();

            var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

            try
            {
                var petId = PetId.Create(command.PetId);
                var pet = volunteer.Value.Pets.FirstOrDefault(v => v.Id == petId);
                if (pet is null)
                    return Errors.General.NotFound(petId).ToErrorList();

                if (pet.PetPhotos.PetPhotos.Count == 0)
                    return command.PetId;

                var petPhotos = pet.PetPhotos.PetPhotos.ToList();
                pet.DeletePhotos();
                await _unitOfWork.SaveChanges(cancellationToken);

                foreach (var petPhoto in petPhotos)
                {
                    var fileInfo = new FileInfo(command.BucketName, petPhoto.Path.Path);
                    var filePathPhoto = await _fileProvider.GetFile(fileInfo, cancellationToken);
                    if (filePathPhoto.IsSuccess)
                        await _fileProvider.Remove(fileInfo, cancellationToken);
                }

                transaction.Commit();
                _logger.Log(LogLevel.Information, "Successful remove medias from pet. Files: {files}", petPhotos);

                return command.PetId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.Log(LogLevel.Critical, "Failed remove files from pet. Exception: {ex}", ex);
                return Error.Failure("remove.pet.files", "Failed remove media from pet").ToErrorList();
            }
        }
    }
}
