
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.EntityIds;
using FluentValidation;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.SetMainPetPhoto
{
    public class SetMainPetPhotoHandler : ICommandHandler<Guid, SetMainPetPhotoCommand>
    {
        private readonly ILogger<SetMainPetPhotoHandler> _logger;
        private readonly IValidator<SetMainPetPhotoCommand> _validator;
        private readonly IVolunteersRepository _repository;
        private readonly IVolunteerUnitOfWork _unitOfWork;

        public SetMainPetPhotoHandler(
            ILogger<SetMainPetPhotoHandler> logger,
            IValidator<SetMainPetPhotoCommand> validator,
            IVolunteersRepository repository,
            IVolunteerUnitOfWork unitOfWork)
        {
            _logger = logger;
            _validator = validator;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            SetMainPetPhotoCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
            {
                return validationResult.ToList();
            }

            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petDeleteResult = volunteerResult.Value.SetMainPetPhoto(command.PetId, command.FileName);
            if (petDeleteResult.IsFailure)
                return petDeleteResult.Error.ToErrorList();

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.Log(LogLevel.Information,
                "Updated PetPhotoLost in Pet {petId}. Current main file - {file}",
                command.PetId,
                command.FileName);

            return command.PetId;
        }
    }
}
