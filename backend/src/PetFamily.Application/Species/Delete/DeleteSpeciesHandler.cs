
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
        private readonly IReadDbContext _readDbContext;

        public DeleteSpeciesHandler(
            ILogger<DeleteSpeciesHandler> logger,
            IUnitOfWork unitOfWork,
            IValidator<DeleteSpeciesCommand> validator,
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

            var petsHaveNotSpeciesResult = CheckPetsDoNotHaveSpecies(speciesResult.Value);

            if (petsHaveNotSpeciesResult.IsFailure)
                return petsHaveNotSpeciesResult.Error.ToErrorList();

            speciesResult.Value.Delete();

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Updated deleted with id {speciesId}", command.SpeciesId);

            return command.SpeciesId;
        }

        private UnitResult<Error> CheckPetsDoNotHaveSpecies(Domain.SpeciesManagement.Species species)
        {
            var pet =  _readDbContext.Pets.FirstOrDefault(p => p.SpeciesId == (Guid)species.Id);

            if (pet is null)
                return Result.Success<Error>();

            return Errors.General.AlreadyUsed(species.Id);
        }
    }
}
