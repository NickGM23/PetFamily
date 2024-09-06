using CSharpFunctionalExtensions;
using PetFamily.Domain.Enums;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models
{
    public class Pet : Shared.Entity<PetId>
    {
        public string Name { get; private set; } = string.Empty;

        public PetBreed Breed { get; private set; } = default!;

        public Description Description { get; private set; } = default!;

        public string Color { get; private set; } = string.Empty;

        public string HealthInfo { get; private set; } = string.Empty;

        public Address Address { get; private set; } = default!;

        public double Weight { get; private set; }

        public double Height { get; private set; }

        public string PhoneNumber { get; private set; } = string.Empty;

        public bool IsCastrated { get; private set; } 

        public DateOnly BirthDay { get; private set; }

        public bool IsVaccinated { get; private set; }

        public HelpStatus HelpStatus { get; private set; }

        public RequisiteList? Requisites { get; private set; } = null!;

        public PetPhotoList? PetPhotos { get; private set; } = null!;

        public DateTime DateCteate { get; private set; }

        private Pet(PetId id) : base(id) { }

        private Pet(PetId id,
                    string name, 
                    PetBreed breed,
                    Description description, 
                    string color, 
                    string healthInfo, 
                    Address address, 
                    double weight,
                    double height,
                    string phoneNumber,
                    bool isCastrated,
                    DateOnly birthDay, 
                    bool isVaccinated,
                    HelpStatus helpStatus) : base(id)
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
        }

        public static Result<Pet, Error> Create(PetId petId, 
                                         string name, 
                                         PetBreed breed,
                                         Description description, 
                                         string color, 
                                         string healthInfo,
                                         Address address,
                                         double weight,
                                         double height, 
                                         string phoneNumber, 
                                         bool isCastrated, 
                                         DateOnly birthDay,
                                         bool isVaccinated, 
                                         HelpStatus helpStatus)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Errors.General.ValueIsInvalid("Name");
            }
            if (string.IsNullOrWhiteSpace(color))
            {
                return Errors.General.ValueIsInvalid("Color");
            }
            if (string.IsNullOrWhiteSpace(healthInfo))
            {
                return Errors.General.ValueIsInvalid("HealthInfo");
            }
            if (weight <= 0)
            {
                return Errors.General.ValueIsInvalid("Weight");
            }
            if (height <= 0)
            {
                return Errors.General.ValueIsInvalid("Height");
            }
            if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length > Constants.MAX_PHONENUMBER_LENGHT)
            {
                return Errors.General.ValueIsInvalid("PhoneNumber");
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
                              helpStatus);
            return pet;
        }

    }
}
