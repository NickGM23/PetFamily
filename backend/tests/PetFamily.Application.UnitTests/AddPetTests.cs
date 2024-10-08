using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.VolunteersManagement;
using PetFamily.Domain.VolunteersManagement.Entities;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Species;
using Microsoft.EntityFrameworkCore;
using PetFamily.Domain.SpeciesManagement.Entities;

namespace PetFamily.Application.UnitTests
{
    public class AddPetTests
    {
        private readonly Mock<IFileProvider> _fileProviderMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IVolunteersRepository> _volunteersRepositoryMock;
        private readonly Mock<ISpeciesRepository> _speciesRepositoryMock;
        private readonly Mock<ILogger<AddPetHandler>> _loggerMock;
        private readonly Mock<IValidator<AddPetCommand>> _validatorMock;
        private readonly Mock<IReadDbContext> _readDbContextMock;

        public AddPetTests()
        {
            _fileProviderMock = new Mock<IFileProvider>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _volunteersRepositoryMock = new Mock<IVolunteersRepository>();
            _loggerMock = new Mock<ILogger<AddPetHandler>>();
            _validatorMock = new Mock<IValidator<AddPetCommand>>();
            _speciesRepositoryMock = new Mock<ISpeciesRepository>();
            _readDbContextMock = new Mock<IReadDbContext>();  
        }

        [Fact]
        public async Task Handle_Should_Add_Pets_To_Volunteer()
        { 
            var ct = CancellationToken.None;
            var volunteer = CreateVolunteer(0);

            var species = CreateSpecies();
            var breed = CreateBreed();
            species.AddBreed(breed);
            ReadDbContextMockSetup(species, breed);

            var command = CreateValidAddPetCommand(volunteer.Id, species.Id, breed.Id);

            _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
                .Returns(Task.CompletedTask);
            _volunteersRepositoryMock.Setup(r => r.GetById(volunteer.Id, ct))
                .ReturnsAsync(volunteer);
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), ct))
                .ReturnsAsync(new ValidationResult());

            var addPetHandler = new AddPetHandler(
                _fileProviderMock.Object,
                _volunteersRepositoryMock.Object,
                _readDbContextMock.Object,
                _unitOfWorkMock.Object,
                _validatorMock.Object,
                _loggerMock.Object,
                _speciesRepositoryMock.Object);

            // act
            var handleResult = await addPetHandler.Handle(command, ct);

            // assert
            var addedPet = volunteer.GetPetById(PetId.Create(handleResult.Value));

            handleResult.IsSuccess.Should().BeTrue();
            volunteer.Pets.Count.Should().Be(1);
            addedPet.IsSuccess.Should().BeTrue();
            addedPet.Value.Id.Value.Should().Be(handleResult.Value);
        }

        [Fact]
        public async Task Handle_Should_Add_Pets_To_Volunteer_If_Pets_Not_Empty()
        {
            // arrange
            const int petsCount = 10;

            var ct = CancellationToken.None;
            var volunteer = CreateVolunteer(petsCount);
            
            var species = CreateSpecies();
            var breed = CreateBreed();
            species.AddBreed(breed);
            ReadDbContextMockSetup(species, breed);

            var command = CreateValidAddPetCommand(volunteer.Id, species.Id, breed.Id);

            _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
                .Returns(Task.CompletedTask);
            _volunteersRepositoryMock.Setup(r => r.GetById(volunteer.Id, ct))
                .ReturnsAsync(volunteer);
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), ct))
                .ReturnsAsync(new ValidationResult());

            var addPetHandler = new AddPetHandler(
                _fileProviderMock.Object,
                _volunteersRepositoryMock.Object,
                _readDbContextMock.Object,
                _unitOfWorkMock.Object,
                _validatorMock.Object,
                _loggerMock.Object,
                _speciesRepositoryMock.Object);

            // act
            var handleResult = await addPetHandler.Handle(command, ct);

            // assert
            var addedPet = volunteer.GetPetById(PetId.Create(handleResult.Value));

            handleResult.IsSuccess.Should().BeTrue();
            volunteer.Pets.Count.Should().Be(petsCount + 1);
            addedPet.IsSuccess.Should().BeTrue();
            addedPet.Value.Id.Value.Should().Be(handleResult.Value);
        }

        [Fact]
        public async Task Handle_Should_Return_Validation_Error_If_Command_Not_Valid()
        {
            // arrange
            var ct = CancellationToken.None;
            var volunteer = CreateVolunteer(0);
            var command = CreateNotValidAddPetCommand(volunteer.Id);

            _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
                .Returns(Task.CompletedTask);
            _volunteersRepositoryMock.Setup(r => r.GetById(volunteer.Id, ct))
                .ReturnsAsync(volunteer);
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), ct))
                .ReturnsAsync(new ValidationResult(
                [
                    new ValidationFailure("phone.number",
                    Errors.General.ValueIsInvalid().Serialize())
                ]));

            var addPetHandler = new AddPetHandler(
                _fileProviderMock.Object,
                _volunteersRepositoryMock.Object,
                _readDbContextMock.Object,
                _unitOfWorkMock.Object,
                _validatorMock.Object,
                _loggerMock.Object,
                _speciesRepositoryMock.Object);

            // act
            var handleResult = await addPetHandler.Handle(command, ct);

            // assert
            handleResult.IsFailure.Should().BeTrue();
            handleResult.Error.First().Type.Should().Be(ErrorType.Validation);
            volunteer.Pets.Count.Should().Be(0);
        }

        [Fact]
        public async Task Handle_Should_Return_Error_If_Volunteer_Does_Not_Exist()
        {
            // arrange
            var ct = CancellationToken.None;
            var volunteer = CreateVolunteer(0);
            var species = CreateSpecies();
            var breed = CreateBreed();
            species.AddBreed(breed);
            var command = CreateValidAddPetCommand(volunteer.Id, species.Id, breed.Id);

            _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
                .Returns(Task.CompletedTask);
            _volunteersRepositoryMock.Setup(r => r.GetById(volunteer.Id, ct))
                .ReturnsAsync(Errors.General.NotFound());
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddPetCommand>(), ct))
                .ReturnsAsync(new ValidationResult());

            var addPetHandler = new AddPetHandler(
                _fileProviderMock.Object,
                _volunteersRepositoryMock.Object,
                _readDbContextMock.Object,
                _unitOfWorkMock.Object,
                _validatorMock.Object,
                _loggerMock.Object,
                _speciesRepositoryMock.Object);

            // act
            var handleResult = await addPetHandler.Handle(command, ct);

            // assert
            handleResult.IsFailure.Should().BeTrue();
            handleResult.Error.First().Type.Should().Be(ErrorType.NotFound);
            volunteer.Pets.Count.Should().Be(0);
        }

        private Volunteer CreateVolunteer(int petsCount)
        {
            var volunteerId = VolunteerId.NewVolunteerId();
            var fullName = FullName.Create("test", "test", "test").Value;
            var email = Email.Create("test@gmail.com").Value;
            var description = Description.Create("test").Value;
            var yearsExperience = YearsExperience.Create(10).Value;
            var phone = PhoneNumber.Create("+380964564545").Value;
            var socialNetworks = new SocialNetworkList(new List<SocialNetwork>());
            var requisites = new RequisiteList(new List<Requisite>());

            var volunteer = Volunteer.Create(
                volunteerId,
                fullName,
                email,
                description,
                yearsExperience,
                phone,
                socialNetworks,
                requisites).Value;

            var pets = Enumerable.Range(0, petsCount)
                .Select(x => CreatePet())
                .ToList();

            foreach (var pet in pets)
            {
                volunteer.AddPet(pet);
            }

            return volunteer;
        }

        private Pet CreatePet()
        {
            var name = Name.Create("test").Value;

            var description = Description.Create("test").Value;

            var petId = PetId.NewPetId();

            var breed = PetBreed.None;

            var color = LowTextLength.Create("Test color").Value;

            var healthInfo = HighTextLength.Create("Test health info").Value;

            var phoneNumber = PhoneNumber.Create("+380975645434").Value;

            var address = Address.Create(
                "test",
                "test",
                "test",
                12345,
                "test",
                "test").Value;

            var requisites = new RequisiteList(new List<Requisite>());

            var pet = Pet.Create(
                petId,
                name,
                breed,
                description,
                color,
                healthInfo,
                address,
                10,
                10,
                phoneNumber,
                false,
                DateOnly.FromDateTime(DateTime.Now),
                true,
                HelpStatus.FoundHome,
                requisites).Value;

            return pet;
        }

        private Domain.SpeciesManagement.Species CreateSpecies()
        {
            return new Domain.SpeciesManagement.Species(
                SpeciesId.NewSpeciesId(), 
                Name.Create("Test name").Value, 
                Description.Create("Test description").Value);
        }

        private Breed CreateBreed()
        {
            var breedId = BreedId.NewBreedId();
            var name = Name.Create("Test name").Value;
            var description = Description.Create("Test description").Value;

            return new Breed(breedId, name, description);
        }

        private AddPetCommand CreateValidAddPetCommand(VolunteerId volunteerId, Guid speciesId, Guid breedId)
        {
            var name = "test";
            var description = "test";
            var color = "test";
            var healthInfo = "Test health info";
            var phoneNumber = "+380975645434";
            var address = new AddressDto("test", "test", "test", 12345, "test", "test");
            var requisites = new List<RequisiteDto>();

            return new AddPetCommand(
                volunteerId.Value,
                name,
                description,
                color,
                healthInfo,
                address,
                10,
                10,
                phoneNumber,
                false,
                DateOnly.FromDateTime(DateTime.Now),
                true,
                "FoundHome",
                requisites,
                speciesId,
                breedId);
        }

        private AddPetCommand CreateNotValidAddPetCommand(VolunteerId volunteerId)
        {
            var name = "test";
            var description = "test";
            var color = "test";
            var healthInfo = "Test health info";
            var phoneNumber = "+3809756454";
            var address = new AddressDto("test", "test", "test", 12345, "test", "test");
            var requisites = new List<RequisiteDto>();

            return new AddPetCommand(
                volunteerId.Value,
                name,
                description,
                color,
                healthInfo,
                address,
                10,
                10,
                phoneNumber,
                false,
                DateOnly.FromDateTime(DateTime.Now),
                true,
                "FoundHome",
                requisites,
                Guid.NewGuid(),
                Guid.NewGuid());
        }

        private void ReadDbContextMockSetup(Domain.SpeciesManagement.Species species, Breed breed)
        {
            var spaciesDto = new SpeciesDto()
            {
                Id = species.Id.Value,
                Name = species.Name.Value,
                Description = species.Description.Value
            };

            var breedDto = new BreedDto(
                breed.Id.Value,
                species.Id.Value,
                breed.Name.Value,
                breed.Description.Value);

            var speciesDtoData = new List<SpeciesDto>() { spaciesDto }.AsQueryable();

            var mockSpeciesDtSet = new Mock<DbSet<SpeciesDto>>();
            mockSpeciesDtSet.As<IQueryable<SpeciesDto>>().Setup(m => m.Provider).Returns(speciesDtoData.Provider);
            mockSpeciesDtSet.As<IQueryable<SpeciesDto>>().Setup(m => m.Expression).Returns(speciesDtoData.Expression);
            mockSpeciesDtSet.As<IQueryable<SpeciesDto>>().Setup(m => m.ElementType).Returns(speciesDtoData.ElementType);
            mockSpeciesDtSet.As<IQueryable<SpeciesDto>>().Setup(m => m.GetEnumerator()).Returns(speciesDtoData.GetEnumerator());

            var breedsDtoDate = new List<BreedDto>() { breedDto }.AsQueryable();

            var mockBreedsDtSet = new Mock<DbSet<BreedDto>>();
            mockBreedsDtSet.As<IQueryable<BreedDto>>().Setup(m => m.Provider).Returns(breedsDtoDate.Provider);
            mockBreedsDtSet.As<IQueryable<BreedDto>>().Setup(m => m.Expression).Returns(breedsDtoDate.Expression);
            mockBreedsDtSet.As<IQueryable<BreedDto>>().Setup(m => m.ElementType).Returns(breedsDtoDate.ElementType);
            mockBreedsDtSet.As<IQueryable<BreedDto>>().Setup(m => m.GetEnumerator()).Returns(breedsDtoDate.GetEnumerator());

            _readDbContextMock.Setup(x => x.Species).Returns(mockSpeciesDtSet.Object);
            _readDbContextMock.Setup(x => x.Breeds).Returns(mockBreedsDtSet.Object);
        }
    }
}
