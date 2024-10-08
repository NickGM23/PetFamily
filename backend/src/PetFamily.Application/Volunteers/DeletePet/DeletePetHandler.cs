
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.Shared;
using FluentValidation;
using PetFamily.Application.Extensions;

namespace PetFamily.Application.Volunteers.DeletePet
{
    public class DeletePetHandler : ICommandHandler<Guid, DeletePetCommand>
    {
        private readonly ILogger<DeletePetHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<DeletePetCommand> _validator;
        private readonly IVolunteersRepository _repository;

        public DeletePetHandler(
            ILogger<DeletePetHandler> logger,
            IUnitOfWork unitOfWork,
            IValidator<DeletePetCommand> validator,
            IVolunteersRepository repository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _repository = repository;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            DeletePetCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
            {
                return validationResult.ToList();
            }

            var volunteerId = VolunteerId.Create(command.VolunteerId);
            var volunteerResult = await _repository.GetById(volunteerId, cancellationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petId = PetId.Create(command.PetId);
            var petDeleteResult = volunteerResult.Value.DeletePet(petId);

            if (petDeleteResult.IsFailure)
                return petDeleteResult.Error.ToErrorList();

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Deleted Pet with ID: {petId}", petId);

            return command.PetId;
        }
    }
}
