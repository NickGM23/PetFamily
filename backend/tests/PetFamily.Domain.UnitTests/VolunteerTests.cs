
using PetFamily.Domain.Enums;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.VolunteersManagement.Entities;
using PetFamily.Domain.VolunteersManagement;
using FluentAssertions;

namespace PetFamily.Domain.UnitTests
{
    public class VolunteerTests
    {
        [Fact]
        public void Add_Pet_With_Empty_Pets_Success_Result()
        {
            // arrange
            var volunteer = CreateVolunteer(0);
            var pet = CreatePet();

            // act
            var result = volunteer.AddPet(pet);

            // assert
            var addedPet = volunteer.GetPetById(pet.Id);

            result.IsSuccess.Should().BeTrue();
            addedPet.IsSuccess.Should().BeTrue();
            addedPet.Value.Id.Should().Be(pet.Id);
            addedPet.Value.SerialNumber.Should().Be(SerialNumber.First);
        }

        [Fact]
        public void Add_Pet_With_Not_Empty_Pets_Success_Result()
        {
            // arrange
            const int petsCount = 3;
            var volunteer = CreateVolunteer(petsCount);
            var petToAdd = CreatePet();

            var firstPet = volunteer.Pets.ToList()[0];
            var secondPet = volunteer.Pets.ToList()[1];
            var thirdPet = volunteer.Pets.ToList()[2];

            // act
            var result = volunteer.AddPet(petToAdd);

            // assert
            result.IsSuccess.Should().BeTrue();
            volunteer.Pets.Count.Should().Be(petsCount + 1);
            petToAdd.SerialNumber.Value.Should().Be(petsCount + 1);
            firstPet.SerialNumber.Value.Should().Be(1);
            secondPet.SerialNumber.Value.Should().Be(2);
            thirdPet.SerialNumber.Value.Should().Be(3);
        }

        [Fact]
        public void MovePet_With_Single_Pets_Should_Not_Move_Return_Success_Result()
        {
            // arrange
            const int petsCount = 1;
            var volunteer = CreateVolunteer(petsCount);
            var pet = volunteer.Pets.First();
            var newSerialNumber = SerialNumber.First;

            // act
            var moveResult = volunteer.MovePet(pet, newSerialNumber);

            // assert
            moveResult.IsSuccess.Should().BeTrue();
            volunteer.Pets.Count.Should().Be(1);
            pet.SerialNumber.Value.Should().Be(SerialNumber.First.Value);
        }

        [Fact]
        public void MovePet_With_Single_Pets_To_Out_Of_Range_Should_Return_Failure_Result()
        {
            // arrange
            const int petsCount = 1;
            var volunteer = CreateVolunteer(petsCount);
            var pet = volunteer.Pets.First();
            var newSerialNumber = SerialNumber.Create(3).Value;

            // act
            var moveResult = volunteer.MovePet(pet, newSerialNumber);

            // assert
            moveResult.IsFailure.Should().BeTrue();
            volunteer.Pets.Count.Should().Be(1);
            pet.SerialNumber.Value.Should().Be(SerialNumber.First.Value);
        }

        [Fact]
        public void MovePet_Should_Move_Others_Forward_If_New_SerialNumber_Is_Lower()
        {
            // arrange
            const int petsCount = 4;
            var volunteer = CreateVolunteer(petsCount);

            var firstPet = volunteer.Pets.ToList()[0];
            var secondPet = volunteer.Pets.ToList()[1];
            var thirdPet = volunteer.Pets.ToList()[2];
            var fourthPet = volunteer.Pets.ToList()[3];

            var newSerialNumber = SerialNumber.Create(2).Value;

            // act
            var moveResult = volunteer.MovePet(fourthPet, newSerialNumber);

            // assert
            moveResult.IsSuccess.Should().BeTrue();
            volunteer.Pets.Count.Should().Be(petsCount);

            firstPet.SerialNumber.Value.Should().Be(1);
            secondPet.SerialNumber.Value.Should().Be(3);
            thirdPet.SerialNumber.Value.Should().Be(4);
            fourthPet.SerialNumber.Value.Should().Be(2);
        }

        [Fact]
        public void MovePet_Should_Move_Others_Backward_If_New_SerialNumber_Is_Greater()
        {
            // arrange
            const int petsCount = 4;
            var volunteer = CreateVolunteer(petsCount);

            var firstPet = volunteer.Pets.ToList()[0];
            var secondPet = volunteer.Pets.ToList()[1];
            var thirdPet = volunteer.Pets.ToList()[2];
            var fourthPet = volunteer.Pets.ToList()[3];

            var newSerialNumber = SerialNumber.Create(4).Value;

            // act
            var moveResult = volunteer.MovePet(secondPet, newSerialNumber);

            // assert
            moveResult.IsSuccess.Should().BeTrue();
            volunteer.Pets.Count.Should().Be(petsCount);

            firstPet.SerialNumber.Value.Should().Be(1);
            thirdPet.SerialNumber.Value.Should().Be(2);
            fourthPet.SerialNumber.Value.Should().Be(3);
            secondPet.SerialNumber.Value.Should().Be(4);
        }

        [Fact]
        public void Move_Pet_Should_Move_Others_Forward_If_New_SerialNumber_Is_First()
        {
            // arrange
            const int petsCount = 4;
            var volunteer = CreateVolunteer(petsCount);

            var firstPet = volunteer.Pets.ToList()[0];
            var secondPet = volunteer.Pets.ToList()[1];
            var thirdPet = volunteer.Pets.ToList()[2];
            var fourthPet = volunteer.Pets.ToList()[3];

            var newSerialNumber = SerialNumber.First;

            // act
            var moveResult = volunteer.MovePet(fourthPet, newSerialNumber);

            // assert
            moveResult.IsSuccess.Should().BeTrue();
            volunteer.Pets.Count.Should().Be(petsCount);

            fourthPet.SerialNumber.Value.Should().Be(1);
            firstPet.SerialNumber.Value.Should().Be(2);
            secondPet.SerialNumber.Value.Should().Be(3);
            thirdPet.SerialNumber.Value.Should().Be(4);
        }

        [Fact]
        public void Move_Pet_Should_Move_Others_Backward_If_New_SerialNumber_Is_Last()
        {
            // arrange
            const int petsCount = 4;
            var volunteer = CreateVolunteer(petsCount);

            var firstPet = volunteer.Pets.ToList()[0];
            var secondPet = volunteer.Pets.ToList()[1];
            var thirdPet = volunteer.Pets.ToList()[2];
            var fourthPet = volunteer.Pets.ToList()[3];

            var newSerialNumber = SerialNumber.Create(petsCount).Value;

            // act
            var moveResult = volunteer.MovePet(firstPet, newSerialNumber);

            // assert
            moveResult.IsSuccess.Should().BeTrue();
            volunteer.Pets.Count.Should().Be(petsCount);

            firstPet.SerialNumber.Value.Should().Be(4);
            secondPet.SerialNumber.Value.Should().Be(1);
            thirdPet.SerialNumber.Value.Should().Be(2);
            fourthPet.SerialNumber.Value.Should().Be(3);
        }

        [Fact]
        public void MovePet_Should_Not_Move_To_The_Same_SerialNumber()
        {
            // arrange
            const int petsCount = 4;
            var volunteer = CreateVolunteer(petsCount);

            var firstPet = volunteer.Pets.ToList()[0];
            var secondPet = volunteer.Pets.ToList()[1];
            var thirdPet = volunteer.Pets.ToList()[2];
            var fourthPet = volunteer.Pets.ToList()[3];

            var newSerialNumber = SerialNumber.Create(2).Value;

            // act
            var moveResult = volunteer.MovePet(secondPet, newSerialNumber);

            // assert
            moveResult.IsSuccess.Should().BeTrue();
            volunteer.Pets.Count.Should().Be(petsCount);

            firstPet.SerialNumber.Value.Should().Be(1);
            secondPet.SerialNumber.Value.Should().Be(2);
            thirdPet.SerialNumber.Value.Should().Be(3);
            fourthPet.SerialNumber.Value.Should().Be(4);
        }


        private Volunteer CreateVolunteer(int petsCount)
        {
            var volunteerId = VolunteerId.NewVolunteerId();
            var fullName = FullName.Create("test", "test", "test").Value;
            var email = Email.Create("email@test.com").Value;
            var description = Description.Create("test").Value;
            var yearsExperience = YearsExperience.Create(10).Value;
            var phoneNumber = PhoneNumber.Create("+380975645434").Value;
            var socialNetworks = new SocialNetworkList(new List<SocialNetwork>());
            var requisites = new RequisiteList(new List<Requisite>());

            var volunteer = Volunteer.Create(
                volunteerId,
                fullName,
                email,
                description,
                yearsExperience,
                phoneNumber,
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
    }
}
