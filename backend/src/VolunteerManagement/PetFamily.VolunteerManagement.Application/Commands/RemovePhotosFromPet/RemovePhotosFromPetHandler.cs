
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using FluentValidation;
using FileInfo = PetFamily.Core.FileProvider.FileInfo;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.FileProvider;
using PetFamily.Core.Extensions;

namespace PetFamily.VolunteerManagement.Application.Commands.RemovePhotosFromPet
{

    public class RemovePhotosFromPetHandler : ICommandHandler<Guid, RemovePhotosFromPetCommand>
    {
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _repository;
        private readonly IVolunteerUnitOfWork _unitOfWork;
        private readonly IValidator<RemovePhotosFromPetCommand> _validator;
        private readonly ILogger<RemovePhotosFromPetHandler> _logger;

        public RemovePhotosFromPetHandler(
            IFileProvider fileProvider,
            IVolunteersRepository repository,
            IVolunteerUnitOfWork unitOfWork,
            IValidator<RemovePhotosFromPetCommand> validator,
            ILogger<RemovePhotosFromPetHandler> logger)
        {
            _fileProvider = fileProvider;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(RemovePhotosFromPetCommand command,
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

                volunteer.Value.DeletePetPhotos(pet);

                await _unitOfWork.SaveChanges(cancellationToken);

                List<FileInfo> filesInfo = [];

                foreach (var petPhoto in petPhotos)
                {
                    filesInfo.Add(new FileInfo(command.BucketName, petPhoto.Path.Path));
                }

                var deleteResult = await _fileProvider.DeleteFiles(filesInfo, cancellationToken);

                if (deleteResult.IsFailure)
                {
                    transaction.Rollback();
                    return deleteResult.Error;
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
