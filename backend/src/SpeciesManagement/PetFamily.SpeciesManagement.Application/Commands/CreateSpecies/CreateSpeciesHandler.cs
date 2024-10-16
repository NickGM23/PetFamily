
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.SpeciesManagement.Application.Commands.CreateSpecies
{
    public class CreateSpeciesHandler : ICommandHandler<Guid, CreateSpeciesCommand>
    {
        private readonly ILogger<CreateSpeciesHandler> _logger;
        private readonly ISpeciesUnitOfWork _unitOfWork;
        private readonly IValidator<CreateSpeciesCommand> _validator;
        private readonly ISpeciesRepository _repository;

        public CreateSpeciesHandler(
            ILogger<CreateSpeciesHandler> logger,
            ISpeciesUnitOfWork unitOfWork,
            IValidator<CreateSpeciesCommand> validator,
            ISpeciesRepository repository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _repository = repository;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            CreateSpeciesCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToList();

            var speciesId = SpeciesId.NewSpeciesId();

            var nameResult = Name.Create(command.Name).Value;

            var descriptionResult = Description.Create(command.Description).Value;

            var species = new Domain.Species(speciesId, nameResult, descriptionResult);

            var speciesNameExistsResult = await _repository.GetByName(species.Name.Value);

            if (speciesNameExistsResult.IsSuccess)
                return Errors.General.AlreadyExists("species", "name", species.Name.Value).ToErrorList();

            await _repository.Add(species);

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Created Species {Name} with id {SpeciesId}", nameResult, species.Id);

            return (Guid)species.Id;
        }
    }
}
