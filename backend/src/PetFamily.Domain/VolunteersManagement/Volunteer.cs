using CSharpFunctionalExtensions;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared.ValueObjects.Ids;
using PetFamily.Domain.VolunteersManagement.Entities;

namespace PetFamily.Domain.VolunteersManagement
{
    public class Volunteer : Shared.Entity<VolunteerId>, ISoftDeletable
    {
        private bool _isDeleted = false;

        private readonly List<Pet> _pets = [];

        public FullName FullName { get; private set; } = null!;

        public Email Email { get; private set; } = default!;

        public Description Description { get; private set; } = default!;

        public YearsExperience YearsExperience { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; } = default!;

        public SocialNetworkList? SocialNetworks { get; private set; } = null!;

        public RequisiteList? Requisites { get; private set; } = null!;

        public IReadOnlyList<Pet> Pets => _pets;

        public int PetsCountNeedHelp() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.NeedHelp);

        public int PetsCountLookingFoHome() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.LookingFoHome);

        public int PetsCountFoundHome() => _pets.Count(p => p.HelpStatus == Enums.HelpStatus.FoundHome);

        private Volunteer(VolunteerId id) : base(id) { }

        private Volunteer(
            VolunteerId id,
            FullName fullName,
            Email email,
            Description description,
            YearsExperience yearsExperience,
            PhoneNumber phoneNumber,
            SocialNetworkList socialNetworks,
            RequisiteList requisites) : base(id)
        {
            FullName = fullName;
            Email = email;
            Description = description;
            YearsExperience = yearsExperience;
            PhoneNumber = phoneNumber;
            SocialNetworks = socialNetworks;
            Requisites = requisites;
        }

        public static Result<Volunteer, Error> Create(
            VolunteerId id,
            FullName fullName,
            Email email,
            Description description,
            YearsExperience yearsExperience,
            PhoneNumber phoneNumber,
            SocialNetworkList socialNetworks,
            RequisiteList requisites)
        {
            var volunteer = new Volunteer(
                id,
                fullName,
                email,
                description,
                yearsExperience,
                phoneNumber,
                socialNetworks,
                requisites);

            return volunteer;
        }

        public void UpdateMainInfo(
            FullName fullName,
            Email email,
            Description description,
            YearsExperience yearsExperience,
            PhoneNumber phoneNumber)
        {
            FullName = fullName;
            Email = email;
            Description = description;
            YearsExperience = yearsExperience;
            PhoneNumber = phoneNumber;
        }

        public void Delete()
        {
            if (_isDeleted == false)
                _isDeleted = true;

            foreach (var pet in _pets)
                pet.Delete();
        }

        public void Restore()
        {
            if (!_isDeleted) return;

            _isDeleted = false;
            foreach (var pet in _pets)
                pet.Restore();
        }

        public void UpdateRequisites(RequisiteList requisites)
        {
            Requisites = requisites;
        }

        public void UpdateSocialNetworks(SocialNetworkList socialNetworks)
        {
            SocialNetworks = socialNetworks;
        }

        public UnitResult<Error> AddPet(Pet pet)
        {
            var serialNumber = SerialNumber.Create(_pets.Count + 1);

            if (serialNumber.IsFailure)
                return serialNumber.Error;

            pet.SetSerialNumber(serialNumber.Value);

            _pets.Add(pet);

            return Result.Success<Error>();
        }

        public UnitResult<Error> MovePet(Pet pet, SerialNumber serialNumber)
        {
            if (_pets.Select(p => p.Id == pet.Id).Any() == false)
                return Errors.General.NotFound(pet.Id);

            if (serialNumber > _pets.Count)
                return Errors.General.ValueIsInvalid("Serial number");

            int indexOfPassedPet = pet.SerialNumber - 1;
            int indexOfNewPet = serialNumber - 1;

            if (indexOfPassedPet == indexOfNewPet)
                return Result.Success<Error>();

            pet.SetSerialNumber(serialNumber);
            Pet savedObj;

            if (indexOfPassedPet > indexOfNewPet)
            {
                _pets[indexOfNewPet].MoveSerialNumberToForward();

                savedObj = _pets[indexOfNewPet];
                _pets[indexOfNewPet] = pet;

                for (int i = indexOfNewPet + 1; i < indexOfPassedPet; i++)
                {
                    _pets[i].MoveSerialNumberToForward();
                    Pet current = _pets[i];

                    _pets[i] = savedObj;
                    savedObj = current;
                }
            }
            else
            {
                _pets[indexOfNewPet].MoveSerialNumberToBackward();

                savedObj = _pets[indexOfNewPet];
                _pets[indexOfNewPet] = pet;

                for (int i = indexOfNewPet - 1; i > indexOfPassedPet; i--)
                {
                    _pets[i].MoveSerialNumberToBackward();
                    Pet current = _pets[i];

                    _pets[i] = savedObj;
                    savedObj = current;
                }
            }

            _pets[indexOfPassedPet] = savedObj;

            return Result.Success<Error>();
        }

        public Result<Pet, Error> GetPetById(PetId petId)
        {
            var pet = _pets.FirstOrDefault(p => p.Id == petId);
            if (pet is null)
                return Errors.General.NotFound(petId.Value);

            return pet;
        }
    }
}
