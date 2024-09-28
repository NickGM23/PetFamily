
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Delete
{
    public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
    {
        private readonly ILogger<DeleteSpeciesHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DeleteSpeciesCommand> _validator;
        private readonly ISpeciesRepository _repository;

        public DeleteSpeciesHandler(
            ILogger<DeleteSpeciesHandler> logger,
            IUnitOfWork unitOfWork,
            IValidator<DeleteSpeciesCommand> validator,
            ISpeciesRepository repository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _repository = repository;
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
