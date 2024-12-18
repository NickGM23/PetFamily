﻿using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.EntityIds;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.Enums;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Domain.Entities
{
    public class Pet : SharedKernel.Entity<PetId>, ISoftDeletable
    {
        private bool _isDeleted = false;

        public Name Name { get; private set; } = default!;

        public PetBreed Breed { get; private set; } = default!;

        public Description Description { get; private set; } = default!;

        public LowTextLength Color { get; private set; } = string.Empty;

        public HighTextLength HealthInfo { get; private set; } = string.Empty;

        public Address Address { get; private set; } = default!;

        public double Weight { get; private set; }

        public double Height { get; private set; }

        public PhoneNumber PhoneNumber { get; private set; } = default!;

        public bool IsCastrated { get; private set; }

        public DateOnly BirthDay { get; private set; }

        public bool IsVaccinated { get; private set; }

        public HelpStatus HelpStatus { get; private set; }

        public RequisiteList? Requisites { get; private set; } = null!;

        public PetPhotoList? PetPhotos { get; private set; } = null!;

        public DateTime DateCteate { get; private set; }

        public SerialNumber SerialNumber { get; private set; } = default!;

        private Pet(PetId id) : base(id) { }

        private Pet(PetId id,
                    Name name,
                    PetBreed breed,
                    Description description,
                    LowTextLength color,
                    HighTextLength healthInfo,
                    Address address,
                    double weight,
                    double height,
                    PhoneNumber phoneNumber,
                    bool isCastrated,
                    DateOnly birthDay,
                    bool isVaccinated,
                    HelpStatus helpStatus,
                    RequisiteList requisites) : base(id)
        {
            Name = name;
            Breed = breed;
            Description = description;
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            Weight = weight;
            Height = height;
            PhoneNumber = phoneNumber;
            IsCastrated = isCastrated;
            BirthDay = birthDay;
            IsVaccinated = isVaccinated;
            HelpStatus = helpStatus;
            DateCteate = DateTime.UtcNow;
            Requisites = requisites;
        }

        public static Result<Pet, Error> Create(PetId petId,
                                         Name name,
                                         PetBreed breed,
                                         Description description,
                                         LowTextLength color,
                                         HighTextLength healthInfo,
                                         Address address,
                                         double weight,
                                         double height,
                                         PhoneNumber phoneNumber,
                                         bool isCastrated,
                                         DateOnly birthDay,
                                         bool isVaccinated,
                                         HelpStatus helpStatus,
                                         RequisiteList requisites)
        {
            if (weight <= 0)
            {
                return Errors.General.ValueIsInvalid(weight.ToString(), "Weight");
            }
            if (height <= 0)
            {
                return Errors.General.ValueIsInvalid(height.ToString(), "Height");
            }

            var pet = new Pet(petId,
                              name,
                              breed,
                              description,
                              color,
                              healthInfo,
                              address,
                              weight,
                              height,
                              phoneNumber,
                              isCastrated,
                              birthDay,
                              isVaccinated,
                              helpStatus,
                              requisites);
            return pet;
        }

        public void Delete()
        {
            if (_isDeleted)
                return;

            _isDeleted = true;
        }

        public void Restore()
        {
            if (!_isDeleted)
                return;

            _isDeleted = false;
        }

        public void UpdatePhotos(IEnumerable<PetPhoto> petPhotos) =>
            PetPhotos = new PetPhotoList(petPhotos);

        public void SetSerialNumber(SerialNumber serialNumber) =>
            SerialNumber = serialNumber;

        public void MoveSerialNumberToForward() =>
            SerialNumber = SerialNumber.Create(SerialNumber.Value + 1).Value;

        public void MoveSerialNumberToBackward() =>
            SerialNumber = SerialNumber.Create(SerialNumber.Value - 1).Value;

        public void UpdateInfo(Pet updatedPet)
        {
            Name = updatedPet.Name;
            Breed = updatedPet.Breed;
            Description = updatedPet.Description;
            Color = updatedPet.Color;
            HealthInfo = updatedPet.HealthInfo;
            Address = updatedPet.Address;
            Weight = updatedPet.Weight;
            Height = updatedPet.Height;
            PhoneNumber = updatedPet.PhoneNumber;
            IsCastrated = updatedPet.IsCastrated;
            BirthDay = updatedPet.BirthDay;
            IsVaccinated = updatedPet.IsVaccinated;
            HelpStatus = updatedPet.HelpStatus;
            Requisites = updatedPet.Requisites;
        }

        public void UpdateStatus(HelpStatus helpStatus)
        {
            HelpStatus = helpStatus;
        }

        internal void RemovePhotos() => PetPhotos = new(new List<PetPhoto>());

        internal UnitResult<Error> SetMainPhoto(string path)
        {
            var filePath = FilePath.Create(path).Value;
            var photo = PetPhotos!.PetPhotos.FirstOrDefault(x => x.Path == filePath);

            if (photo == null)
                return Errors.General.NotFound("photo.not.found", $"Photo '{path}' not found");

            var newPetPhotoList = new List<PetPhoto>();
            foreach (var petPhoto in PetPhotos!.PetPhotos)
            {
                newPetPhotoList.Add(petPhoto.Path != filePath
                    ? PetPhoto.Create(petPhoto.Path, false).Value
                    : PetPhoto.Create(petPhoto.Path, true).Value);
            }
            PetPhotos = new(newPetPhotoList.OrderByDescending(x => x.IsMain));

            return Result.Success<Error>();
        }
    }
}
