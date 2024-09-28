
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.VolunteersManagement.Entities;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Extensions;
using PetFamily.Domain.Models;
using PetFamily.Domain.Enums;
using PetFamily.Application.Database;
using PetFamily.Application.Species;

namespace PetFamily.Application.Volunteers.AddPet
{
    public class AddPetHandler
    {
        private const string BUCKET_NAME = "photos";
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddPetCommand> _validator;
        private readonly ILogger<AddPetHandler> _logger;
        private readonly ISpeciesRepository _speciesRepository;


        public AddPetHandler(
            IFileProvider fileProvider,
            IVolunteersRepository volunteersRepository,
            IUnitOfWork unitOfWork,
            IValidator<AddPetCommand> validator,
            ILogger<AddPetHandler> logger,
            ISpeciesRepository speciesRepository)
        {
            _fileProvider = fileProvider;
            _repository = volunteersRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
            _speciesRepository = speciesRepository;
        }

        public async Task<Result<Guid, ErrorList>> Handle(
            AddPetCommand command,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToList();

            var volunteerResult = await _repository
                .GetById(command.VolunteerId, cancellationToken);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            var speciesResult = await _speciesRepository
                .GetById(command.SpeciesId, cancellationToken);
            if (speciesResult.IsFailure)
                return speciesResult.Error.ToErrorList();

            var breedResult = speciesResult.Value.Breeds.FirstOrDefault(x => x.Id == command.BreedId);
            if (breedResult == null)
                return Errors.General.NotFound(command.BreedId).ToErrorList();

            //var speciesResult = _readDbContext.Species.Any(x => x.Id == command.SpeciesId);

            //if (speciesResult.IsFailure)
            //    return Errors.General.NotFound(command.SpeciesId).ToErrorList();

            //var breedExists = _readDbContext.Breeds.Any(x => x.Id == command.BreedId);

            //if (isBreedExists == false)
            //    return Errors.General.NotFound(command.BreedId).ToErrorList();

            var pet = InitPet(command);
            volunteerResult.Value.AddPet(pet);

            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation("Pet added with id: {PetId}.", pet.Id.Value);

            return pet.Id.Value;
        }

        private Pet InitPet(AddPetCommand command)
        {
            var petId = PetId.NewPetId();
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
