
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateRequisites
{
    public class UpdateRequisitesHandler
    {
        private readonly IVolunteersRepository _repository;
        private readonly IValidator<UpdateRequisitesCommand> _validator;
        private readonly ILogger<UpdateRequisitesHandler> _logger;

        public UpdateRequisitesHandler(
            IVolunteersRepository repository,
            IValidator<UpdateRequisitesCommand> validator,
            ILogger<UpdateRequisitesHandler> logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdateRequisitesCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToList();

            var volunteerResult = await _repository.GetById(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            List<Requisite> requisites = [];
            foreach (var requisite in command.Requisites)
            {
                var description = Description.Create(requisite.Description).Value;

                requisites.Add(Requisite.Create(requisite.Name, requisite.Description).Value);
            }

            var requisitesToUpdate = RequisiteList.Create(requisites);
            if (requisitesToUpdate.IsFailure)
                return requisitesToUpdate.Error.ToErrorList();

            volunteerResult.Value.UpdateRequisites(requisitesToUpdate.Value);

            await _repository.Save(volunteerResult.Value, cancellationToken);

            _logger.LogInformation("Requisites of volunteer with {id} has been updated", command.VolunteerId);

            return command.VolunteerId;
        }
    }
}
