
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Extensions;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SpeciesManagement.Domain.Entities;

namespace PetFamily.SpeciesManagement.Application.Commands.AddBreed
{
    public class AddBreedHandler : ICommandHandler<Guid, AddBreedCommand>
    {
        private readonly ILogger<AddBreedHandler> _logger;
        private readonly ISpeciesUnitOfWork _unitOfWork;
        private readonly IValidator<AddBreedCommand> _validator;
        private readonly ISpeciesRepository _repository;

        public AddBreedHandler(
            ILogger<AddBreedHandler> logger,
            ISpeciesUnitOfWork unitOfWork,
            IValidator<AddBreedCommand> validator,
            ISpeciesRepository repository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _repository = repository;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            AddBreedCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ToList();

            var speciesResult = await _repository
                .GetById(command.SpeciesId, cancellationToken);
            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();

            var breed = InitBreed(command);

            var addBreedResult = speciesResult.Value.AddBreed(breed);

            if (addBreedResult.IsFailure)
                return addBreedResult.Error.ToErrorList();

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Breed added with id: {BredId}.", breed.Id.Value);

            return breed.Id.Value;
        }

        private Breed InitBreed(AddBreedCommand command)
        {
            var breedId = BreedId.NewBreedId();
            var name = Name.Create(command.Name).Value;
            var description = Description.Create(command.Description).Value;

            return new Breed(breedId, name, description);
        }
    }
}
