
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Commands.Delete
{
    public class DeleteVolunteerHandler : ICommandHandler<Guid, DeleteVolunteerCommand>
    {
        private readonly IVolunteersRepository _repository;
        private readonly IVolunteerUnitOfWork _unitOfWork;
        private readonly IValidator<DeleteVolunteerCommand> _validator;
        private readonly ILogger<DeleteVolunteerHandler> _logger;

        public DeleteVolunteerHandler(
            IVolunteersRepository repository,
            IVolunteerUnitOfWork unitOfWork,
            IValidator<DeleteVolunteerCommand> validator,
            ILogger<DeleteVolunteerHandler> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            DeleteVolunteerCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
            {
                return validationResult.ToList();
            }

            var volunteerResult = await _repository.GetById(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            volunteerResult.Value.Delete();

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Updated deleted with id {volunteerId}", command.VolunteerId);

            return command.VolunteerId;
        }
    }
}
