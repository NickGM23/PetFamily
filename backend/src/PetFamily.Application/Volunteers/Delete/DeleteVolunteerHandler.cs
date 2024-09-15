﻿
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Delete
{
    public class DeleteVolunteerHandler
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<DeleteVolunteerCommand> _validator;
        private readonly ILogger<DeleteVolunteerHandler> _logger;

        public DeleteVolunteerHandler(
            IVolunteersRepository repository,
            IValidator<DeleteVolunteerCommand> validator,
            ILogger<DeleteVolunteerHandler> logger)
        {
            _repository = repository;
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

            await _repository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Updated deleted with id {volunteerId}", command.VolunteerId);

            return command.VolunteerId;
        }
    }
}
