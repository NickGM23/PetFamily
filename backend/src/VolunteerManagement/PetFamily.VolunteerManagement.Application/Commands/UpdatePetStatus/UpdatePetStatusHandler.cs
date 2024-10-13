
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.VolunteerManagement.Domain.Enums;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdatePetStatus
{
    public class UpdatePetStatusHandler : ICommandHandler<Guid, UpdatePetStatusCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly IVolunteerUnitOfWork _unitOfWork;
        private readonly IValidator<UpdatePetStatusCommand> _validator;
        private readonly ILogger<UpdatePetStatusHandler> _logger;

        public UpdatePetStatusHandler(
            IVolunteersRepository repository,
            IVolunteerUnitOfWork unitOfWork,
            IValidator<UpdatePetStatusCommand> validator,
            ILogger<UpdatePetStatusHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdatePetStatusCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return validationResult.ToList();

            var volunteerResult = await _repository.GetById(command.VolunteerId, cancellationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petResult = volunteerResult.Value.GetPetById(command.PetId);

            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            var helpStatus = Enum.Parse<HelpStatus>(command.Status);

            volunteerResult.Value.UpdatePetStatus(petResult.Value, helpStatus);

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Help status was updated for pet with id {petId}", petResult.Value.Id);

            return (Guid)petResult.Value.Id;
        }
    }
}
