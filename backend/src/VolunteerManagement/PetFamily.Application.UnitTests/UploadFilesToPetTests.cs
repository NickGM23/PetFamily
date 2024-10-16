using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.Enums;
using PetFamily.VolunteerManagement.Domain.ValueObjects;
using PetFamily.VolunteerManagement.Domain;
using PetFamily.VolunteerManagement.Domain.Entities;
using PetFamily.Core.Dtos;
using PetFamily.Core.FileProvider;
using PetFamily.VolunteerManagement.Application;
using PetFamily.VolunteerManagement.Application.Commands.UploadFilesToPet;

namespace PetFamily.Application.UnitTests
{
    public class UploadFilesToPetTests
    {
        private readonly Mock<IFileProvider> _fileProviderMock;
        private readonly Mock<IVolunteerUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IVolunteersRepository> _volunteersRepositoryMock;
        private readonly Mock<ILogger<UploadFilesToPetHandler>> _loggerMock;
        private readonly Mock<IValidator<UploadFilesToPetCommand>> _validatorMock;

        public UploadFilesToPetTests()
        {
            _fileProviderMock = new Mock<IFileProvider>();
            _unitOfWorkMock = new Mock<IVolunteerUnitOfWork>();
            _volunteersRepositoryMock = new Mock<IVolunteersRepository>();
            _loggerMock = new Mock<ILogger<UploadFilesToPetHandler>>();
            _validatorMock = new Mock<IValidator<UploadFilesToPetCommand>>();
        }

        [Fact]
        public async Task Handle_Should_Return_Validation_Error_If_Command_Not_Valid()
        {
            // arrange
            const int petCount = 1;
            const int filesCount = 3;

            var volunteer = CreateVolunteer(petCount);
            var pet = volunteer.Pets.First();

            var ct = new CancellationTokenSource().Token;

            var files = CreateFileDtos(filesCount);

            var command = new UploadFilesToPetCommand(volunteer.Id.Value, pet.Id.Value, "test", files);

            _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
                .Returns(Task.CompletedTask);

            _volunteersRepositoryMock.Setup(r => r.GetById(volunteer.Id, ct))
                .ReturnsAsync(volunteer);

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UploadFilesToPetCommand>(), ct))
                .ReturnsAsync(new ValidationResult(
                [
                    new ValidationFailure(
                    "fileName",
                    Errors.General.ValueIsInvalid().Serialize())
                ]));

            _fileProviderMock.Setup(f => f.Uploads(It.IsAny<IEnumerable<FileData>>(), ct))
                .ReturnsAsync(new List<string>());

            var handler = new UploadFilesToPetHandler(
                _fileProviderMock.Object,
                _volunteersRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _validatorMock.Object,
                _loggerMock.Object);

            // act
            var handleResult = await handler.Handle(command, ct);

            // assert
            handleResult.IsFailure.Should().BeTrue();
            handleResult.Error.First().Type.Should().Be(ErrorType.Validation);
            pet.PetPhotos.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Return_Error_If_Volunteer_Not_Found()
        {
            // arrange
            const int petCount = 1;
            const int filesCount = 3;

            var volunteer = CreateVolunteer(petCount);
            var pet = volunteer.Pets.First();

            var ct = new CancellationTokenSource().Token;

            var files = CreateFileDtos(filesCount);

            var command = new UploadFilesToPetCommand(volunteer.Id.Value, pet.Id.Value, "test", files);

            _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
                .Returns(Task.CompletedTask);

            _volunteersRepositoryMock.Setup(r => r.GetById(volunteer.Id, ct))
                .ReturnsAsync(Errors.General.NotFound());

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UploadFilesToPetCommand>(), ct))
                .ReturnsAsync(new ValidationResult());

            _fileProviderMock.Setup(f => f.Uploads(It.IsAny<IEnumerable<FileData>>(), ct))
                .ReturnsAsync(new List<string>());

            var handler = new UploadFilesToPetHandler(
                _fileProviderMock.Object,
                _volunteersRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _validatorMock.Object,
                _loggerMock.Object);

            // act
            var handleResult = await handler.Handle(command, ct);

            // assert
            handleResult.IsFailure.Should().BeTrue();
            handleResult.Error.First().Type.Should().Be(ErrorType.NotFound);
            pet.PetPhotos.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Return_Error_If_Pet_Not_Found()
        {
            // arrange
            const int petCount = 1;
            const int filesCount = 3;

            var volunteer = CreateVolunteer(petCount);
            var pet = volunteer.Pets.First();

            var ct = new CancellationTokenSource().Token;

            var files = CreateFileDtos(filesCount);

            var invalidPetId = Guid.Empty;

            var command = new UploadFilesToPetCommand(volunteer.Id.Value, invalidPetId, "test", files);

            _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
                .Returns(Task.CompletedTask);

            _volunteersRepositoryMock.Setup(r => r.GetById(volunteer.Id, ct))
                .ReturnsAsync(volunteer);

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UploadFilesToPetCommand>(), ct))
                .ReturnsAsync(new ValidationResult());

            _fileProviderMock.Setup(f => f.Uploads(It.IsAny<IEnumerable<FileData>>(), ct))
                .ReturnsAsync(new List<string>());

            var handler = new UploadFilesToPetHandler(
                _fileProviderMock.Object,
                _volunteersRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _validatorMock.Object,
                _loggerMock.Object);

            // act
            var handleResult = await handler.Handle(command, ct);

            // assert
            handleResult.IsFailure.Should().BeTrue();
            handleResult.Error.First().Type.Should().Be(ErrorType.NotFound);
            pet.PetPhotos.Should().BeNull();
        }

        [Fact]
        public async Task Handle_Should_Upload_Files_To_Pet_Successfully()
        {
            // arrange
            const int petCount = 1;
            const int filesCount = 3;

            var volunteer = CreateVolunteer(petCount);
            var pet = volunteer.Pets.First();

            var ct = new CancellationTokenSource().Token;

            var files = CreateFileDtos(filesCount);

            var command = new UploadFilesToPetCommand(volunteer.Id.Value, pet.Id.Value, "test", files);

            _unitOfWorkMock.Setup(u => u.SaveChanges(ct))
                .Returns(Task.CompletedTask);

            _volunteersRepositoryMock.Setup(r => r.GetById(volunteer.Id, ct))
                .ReturnsAsync(volunteer);

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UploadFilesToPetCommand>(), ct))
                .ReturnsAsync(new ValidationResult());

            _fileProviderMock.Setup(f => f.Uploads(It.IsAny<IEnumerable<FileData>>(), ct))
                .ReturnsAsync(Enumerable.Range(1, filesCount)
                    .Select(i => Guid.NewGuid() + ".png")
                    .ToList());

            var handler = new UploadFilesToPetHandler(
                _fileProviderMock.Object,
                _volunteersRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _validatorMock.Object,
                _loggerMock.Object);

            // act
            var handleResult = await handler.Handle(command, ct);

            // assert
            handleResult.IsSuccess.Should().BeTrue();
            handleResult.Value.Should().Be(pet.Id.Value);
            pet.PetPhotos.PetPhotos.Count.Should().Be(filesCount);
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

        private IEnumerable<UploadFileDto> CreateFileDtos(int count)
        => Enumerable.Range(1, count)
                .Select(i => new UploadFileDto(Stream.Null, $"file{i}.png"));
    }
}
