
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Database;
using PetFamily.SharedKernel;
using PetFamily.VolunteerManagement.Contracts;

namespace PetFamily.SpeciesManagement.Application.Commands.Delete
{
    public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
    {
        private readonly ILogger<DeleteSpeciesHandler> _logger;
        private readonly ISpeciesUnitOfWork _unitOfWork;
        private readonly IValidator<DeleteSpeciesCommand> _validator;
        private readonly ISpeciesRepository _repository;
        private readonly IVolunteerContract _volunteerContract;

        public DeleteSpeciesHandler(
            ILogger<DeleteSpeciesHandler> logger,
            ISpeciesUnitOfWork unitOfWork,
            IValidator<DeleteSpeciesCommand> validator,
            ISpeciesRepository repository,
            IVolunteerContract volunteerContract)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _repository = repository;
            _volunteerContract = volunteerContract;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            DeleteSpeciesCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
            {
                return validationResult.ToList();
            }

            var isSpeciesUsed = await _volunteerContract.IsPetsUsedSpecies(command.SpeciesId, cancellationToken);
            if (isSpeciesUsed.IsSuccess)
                return Errors.General.AlreadyUsed(command.SpeciesId).ToErrorList();

            var speciesResult = await _repository.GetById(command.SpeciesId, cancellationToken);
            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();

            speciesResult.Value.Delete();

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Updated deleted with id {speciesId}", command.SpeciesId);

            return command.SpeciesId;
        }
    }
}
