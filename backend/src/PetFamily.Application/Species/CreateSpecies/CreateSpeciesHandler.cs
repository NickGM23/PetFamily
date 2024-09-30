
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.Species.CreateSpecies
{
    public class CreateSpeciesHandler : ICommandHandler<Guid, CreateSpeciesCommand>
    {
        private readonly ILogger<CreateSpeciesHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateSpeciesCommand> _validator;
        private readonly ISpeciesRepository _repository;
        private readonly IReadDbContext _readDbContext;

        public CreateSpeciesHandler(
            ILogger<CreateSpeciesHandler> logger,
            IUnitOfWork unitOfWork,
            IValidator<CreateSpeciesCommand> validator,
            ISpeciesRepository repository,
            IReadDbContext readDbContext)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _repository = repository;
            _readDbContext = readDbContext;
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

            var species = new Domain.SpeciesManagement.Species(speciesId, nameResult, descriptionResult);

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
