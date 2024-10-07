using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Volunteers.DeletePet;
using PetFamily.Application.Volunteers.RemovePhotosFromPet;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.Volunteers.ForceDeletePet
{
    public class ForceDeletePetHandler : ICommandHandler<Guid, RemovePhotosFromPetCommand>
    {
        private readonly IFileProvider _fileProvider;
        private readonly ILogger<DeletePetHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RemovePhotosFromPetCommand> _validator;
        private readonly IVolunteersRepository _repository;

        public ForceDeletePetHandler(
            IFileProvider fileProvider,
            ILogger<DeletePetHandler> logger, 
            IUnitOfWork unitOfWork,
            IValidator<RemovePhotosFromPetCommand> validator,
            IVolunteersRepository repository)
        {
            _fileProvider = fileProvider;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _repository = repository;
        }
        public async Task<Result<Guid, ErrorList>> Handle(
            RemovePhotosFromPetCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
            {
                return validationResult.ToList();
            }

            var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

            try
            {
                var volunteerId = VolunteerId.Create(command.VolunteerId);
                var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

                if (volunteerResult.IsFailure)
                    return volunteerResult.Error.ToErrorList();

                var petId = PetId.Create(command.PetId);
                var petResult = volunteerResult.Value.Pets.FirstOrDefault(i => i.Id.Value == command.PetId);
                if (petResult == null)
                    return Errors.General.NotFound(command.PetId).ToErrorList();

                var petDeleteResult = volunteerResult.Value.ForceDeletePet(petResult.Id);

                if (petDeleteResult.IsFailure)
                    return petDeleteResult.Error.ToErrorList();

                var petPhotos = petDeleteResult.Value.PetPhotos.PetPhotos.ToList();

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
                _logger.LogInformation("Force deleted Pet with ID: {petId}", petId);

                return command.PetId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.Log(LogLevel.Critical, "Failed force delete Pet with ID: {petId}. Exception: {ex}", command.PetId, ex);
                return Error.Failure("force.delete.pet", "Failed force delete Pet").ToErrorList();
            }
        }
    }
}
