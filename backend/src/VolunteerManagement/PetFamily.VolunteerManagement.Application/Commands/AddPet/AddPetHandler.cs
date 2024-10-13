
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.Enums;
using PetFamily.VolunteerManagement.Domain.ValueObjects;
using PetFamily.VolunteerManagement.Domain.Entities;
using PetFamily.Core.FileProvider;
using PetFamily.Core.Extensions;
using PetFamily.SpeciesManagement.Application;
using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.AddPet
{
    public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
    {
        private const string BUCKET_NAME = "photos";
        private readonly IFileProvider _fileProvider;
        private readonly IVolunteersRepository _repository;
        private readonly ISpeciesReadDbContext _readDbContext;
        private readonly IVolunteerUnitOfWork _unitOfWork;
        private readonly IValidator<AddPetCommand> _validator;
        private readonly ILogger<AddPetHandler> _logger;
        private readonly ISpeciesRepository _speciesRepository;


        public AddPetHandler(
            IFileProvider fileProvider,
            IVolunteersRepository volunteersRepository,
            ISpeciesReadDbContext readDbContext,
            IVolunteerUnitOfWork unitOfWork,
            IValidator<AddPetCommand> validator,
            ILogger<AddPetHandler> logger,
            ISpeciesRepository speciesRepository)
        {
            _fileProvider = fileProvider;
            _repository = volunteersRepository;
            _readDbContext = readDbContext;
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

            var isSpeciesExists = _readDbContext.Species.Any(s => s.Id == command.SpeciesId);

            if (isSpeciesExists == false)
                return Errors.General.NotFound(command.SpeciesId).ToErrorList();

            var isBreedExists = _readDbContext.Breeds.Any(b =>
                b.Id == command.BreedId && b.SpeciesId == command.SpeciesId);

            if (isBreedExists == false)
                return Errors.General.NotFound(command.BreedId).ToErrorList();

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
