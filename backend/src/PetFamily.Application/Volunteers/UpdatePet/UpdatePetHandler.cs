
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Species;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteersManagement.Entities;
using System.Drawing;
using PetFamily.Application.Extensions;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared.ValueObjects.Ids;

namespace PetFamily.Application.Volunteers.UpdatePet
{
    public class UpdatePetHandler : ICommandHandler<Guid, UpdatePetCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IVolunteersRepository _volunteersRepository;
        private readonly ILogger<UpdatePetHandler> _logger;
        private readonly IValidator<UpdatePetCommand> _validator;

        public UpdatePetHandler(
            IUnitOfWork unitOfWork,
            ISpeciesRepository speciesRepository,
            IVolunteersRepository volunteersRepository,
            ILogger<UpdatePetHandler> logger,
            IValidator<UpdatePetCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _speciesRepository = speciesRepository;
            _volunteersRepository = volunteersRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            UpdatePetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (validationResult.IsValid == false)
                return validationResult.ToList();

            var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var petResult = volunteerResult.Value.GetPetById(command.PetId);

            if (petResult.IsFailure)
                return petResult.Error.ToErrorList();

            var speciesResult = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);

            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();

            var breedResult = speciesResult.Value.GetBreedById(command.BreedId);

            if (breedResult.IsFailure)
                return breedResult.Error.ToErrorList();

            if (!Enum.TryParse(command.HelpStatus, out HelpStatus helpStatus))
                return Errors.General.ValueIsInvalid("Help status").ToErrorList();

            volunteerResult.Value.UpdatePet(InitPet(petResult.Value.Id, command));

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Pet was updated with id {petId}", command.PetId);

            return command.PetId;
        }

        private Pet InitPet(PetId petId, UpdatePetCommand command)
        {
            var name = Name.Create(command.Name).Value;
            var petBreed = PetBreed.Create(SpeciesId.Create(command.SpeciesId), command.BreedId).Value;
            var description = Description.Create(command.Description).Value;
            var color = LowTextLength.Create(command.Color).Value;
            var healthInfo = HighTextLength.Create(command.HealthInfo).Value;
            var address = Address.Create(
                command.Address.Country,
                command.Address.City,
                command.Address.Street,
                command.Address.PostalCode,
                command.Address.HouseNumber,
                command.Address.FlatNumber).Value;
            var weight = command.Weight;
            var height = command.Height;
            var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
            var isCastrated = command.IsCastrated;
            var birthDate = command.BirthDate;
            var isVaccinated = command.IsVaccinated;
            var status = Enum.Parse<HelpStatus>(command.HelpStatus);
            RequisiteList requisites = new(command.Requisites.
                Select(r => Requisite.Create(r.Name, r.Description).Value));

            return Pet.Create(
                petId,
                name,
                petBreed,
                description,
                color,
                healthInfo,
                address,
                weight,
                height,
                phoneNumber,
                isCastrated,
                birthDate,
                isVaccinated,
                status,
                requisites).Value;
        }
    }
}
